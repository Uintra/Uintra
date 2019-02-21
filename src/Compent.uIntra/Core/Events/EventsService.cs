using System;
using System.Collections.Generic;
using System.Linq;
using Compent.CommandBus;
using Compent.Extensions;
using Compent.Uintra.Core.Notification;
using Compent.Uintra.Core.Search.Entities;
using Compent.Uintra.Core.UserTags.Indexers;
using Uintra.CentralFeed;
using Uintra.Comments;
using Uintra.Core.Activity;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.LinkPreview;
using Uintra.Core.Links;
using Uintra.Core.Location;
using Uintra.Core.Media;
using Uintra.Core.Permissions;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Core.User.Permissions;
using Uintra.Events;
using Uintra.Groups;
using Uintra.Likes;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Search;
using Uintra.Subscribe;
using Uintra.Tagging.UserTags;
using static Uintra.Notification.Configuration.NotificationTypeEnum;

namespace Compent.Uintra.Core.Events
{
    public class EventsService : IntranetActivityService<Event>,
        IEventsService<Event>,
        IFeedItemService,
        ISubscribableService,
        INotifyableService,
        IReminderableService<Event>,
        IIndexer,
        IHandle<VideoConvertedCommand>
    {
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly ISubscribeService _subscribeService;
        private readonly IBasePermissionsService _permissionsService;
        private readonly INotificationsService _notificationService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IElasticUintraActivityIndex _activityIndex;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IActivityLinkService _linkService;
        private readonly UserTagService _userTagService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivitySubscribeSettingService _activitySubscribeSettingService;
        private readonly IFeedTypeProvider _feedTypeProvider;
        private readonly IGroupService _groupService;
        private readonly INotifierDataBuilder _notifierDataBuilder;

        public EventsService(
            IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            ICommentsService commentsService,
            ILikesService likesService,
            ISubscribeService subscribeService,
            IBasePermissionsService permissionsService,
            INotificationsService notificationService,
            IMediaHelper mediaHelper,
            IElasticUintraActivityIndex activityIndex,
            IDocumentIndexer documentIndexer,
            IActivityTypeProvider activityTypeProvider,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLinkService linkService,
            IActivityLocationService activityLocationService,
            UserTagService userTagService,
            IActivitySubscribeSettingService activitySubscribeSettingService,
            IFeedTypeProvider feedTypeProvider,
            IActivityLinkPreviewService activityLinkPreviewService,
            IGroupService groupService,
            INotifierDataBuilder notifierDataBuilder)
            : base(
                intranetActivityRepository,
                cacheService,
                activityTypeProvider,
                intranetMediaService,
                activityLocationService,
                activityLinkPreviewService)
        {
            _intranetMemberService = intranetMemberService;
            _commentsService = commentsService;
            _likesService = likesService;
            _subscribeService = subscribeService;
            _permissionsService = permissionsService;
            _notificationService = notificationService;
            _mediaHelper = mediaHelper;
            _activityIndex = activityIndex;
            _documentIndexer = documentIndexer;
            _intranetMediaService = intranetMediaService;
            _groupActivityService = groupActivityService;
            _linkService = linkService;
            _userTagService = userTagService;
            _activitySubscribeSettingService = activitySubscribeSettingService;
            _feedTypeProvider = feedTypeProvider;
            _groupService = groupService;
            _notifierDataBuilder = notifierDataBuilder;
        }

        public override Enum Type => IntranetActivityTypeEnum.Events;
        public override PermissionActivityTypeEnum PermissionActivityType => PermissionActivityTypeEnum.Events;

        public IEnumerable<Event> GetPastEvents()
        {
            return GetAll().Where(@event => !IsActual(@event) && !@event.IsHidden);
        }

        public IEnumerable<Event> GetComingEvents(DateTime fromDate)
        {
            var events = GetAll()
                .Where(e => e.StartDate > fromDate && IsActualPublishDate(e))
                .OrderBy(e => e.StartDate);
            return events;
        }

        public void Hide(Guid id)
        {
            var @event = Get(id);
            @event.IsHidden = true;
            Save(@event);
        }

        public bool CanSubscribe(Guid activityId)
        {
            var @event = Get(activityId);
            return IsActual(@event) && @event.CanSubscribe;
        }

        public MediaSettings GetMediaSettings() => _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.EventsContent);

        public FeedSettings GetFeedSettings()
        {
            return new FeedSettings
            {
                Type = _feedTypeProvider[Type.ToInt()],
                Controller = "Events",
                HasSubscribersFilter = true,
                HasPinnedFilter = true
            };
        }

        public override bool CanEdit(IIntranetActivity activity)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            var isWebmaster = currentMember.Group.Id == IntranetRolesEnum.WebMaster.ToInt();

            if (isWebmaster) return true;

            var ownerId = Get(activity.Id).OwnerId;
            var isOwner = ownerId == currentMember.Id;

            var isMemberHasPermissions = _permissionsService.Check(currentMember, PermissionActivityType, PermissionActionEnum.Edit);

            return isOwner && isMemberHasPermissions;
        }

        public IEnumerable<IFeedItem> GetItems() => GetOrderedActualItems();

        public override Guid Create(IIntranetActivity activity)
        {
            return base.Create(activity, activityId =>
            {
                var subscribeSettings = Map(activity);
                subscribeSettings.ActivityId = activityId;
                _activitySubscribeSettingService.Create(subscribeSettings);
            });
        }

        public override void Save(IIntranetActivity activity)
        {
            base.Save(activity, savedActivity => _activitySubscribeSettingService.Save(Map(savedActivity)));
        }

        public override void Delete(Guid id)
        {
            _activitySubscribeSettingService.Delete(id);
            base.Delete(id);
        }

        private IOrderedEnumerable<Event> GetOrderedActualItems() =>
            GetManyActual().OrderByDescending(i => i.PublishDate);

        protected override void MapBeforeCache(IList<Event> cached)
        {
            foreach (var activity in cached)
            {
                var entity = activity;
                entity.GroupId = _groupActivityService.GetGroupId(activity.Id);
                _subscribeService.FillSubscribers(entity);
                _commentsService.FillComments(entity);
                _likesService.FillLikes(entity);
                _activitySubscribeSettingService.FillSubscribeSettings(entity);
            }
        }

        protected override void UpdateCache()
        {
            base.UpdateCache();
            FillIndex();
        }

        public override Event UpdateActivityCache(Guid id)
        {
            var cachedEvent = Get(id);
            var @event = base.UpdateActivityCache(id);
            if (IsCacheable(@event) && (@event.GroupId is null || _groupService.IsActivityFromActiveGroup(@event)))
            {
                _activityIndex.Index(Map(@event));
                _documentIndexer.Index(@event.MediaIds);
                return @event;
            }

            if (cachedEvent == null) return null;

            _activityIndex.Delete(id);
            _documentIndexer.DeleteFromIndex(cachedEvent.MediaIds);
            _mediaHelper.DeleteMedia(cachedEvent.MediaIds);
            return null;
        }

        public override bool IsActual(IIntranetActivity activity)
        {
            return base.IsActual(activity) && IsActualPublishDate((Event) activity);
        }

        public void UnSubscribe(Guid userId, Guid activityId)
        {
            _subscribeService.Unsubscribe(userId, activityId);
            UpdateActivityCache(activityId);
        }

        public void UpdateNotification(Guid id, bool value)
        {
            var subscribe = _subscribeService.UpdateNotification(id, value);
            UpdateActivityCache(subscribe.ActivityId);
        }

        public bool CanEditSubscribe(Guid activityId) => !Get(activityId).Subscribers.Any();

        public void Notify(Guid entityId, Enum notificationType)
        {
            NotifierData notifierData;

            if (notificationType.In(CommentAdded, CommentEdited, CommentLikeAdded, CommentReplied))
            {
                var comment = _commentsService.Get(entityId);
                var parentActivity = Get(comment.ActivityId);
                notifierData = _notifierDataBuilder.GetNotifierData(comment, parentActivity, notificationType);
            }
            else
            {
                var activity = Get(entityId);
                notifierData = _notifierDataBuilder.GetNotifierData(activity, notificationType);
            }

            _notificationService.ProcessNotification(notifierData);
        }

        public ISubscribable Subscribe(Guid userId, Guid activityId)
        {
            _subscribeService.Subscribe(userId, activityId);
            return UpdateActivityCache(activityId);
        }

        public Event GetActual(Guid id)
        {
            var @event = Get(id);
            return !@event.IsHidden ? @event : null;
        }

        public void FillIndex()
        {
            var activities = GetAll().Where(IsCacheable);
            var searchableActivities = activities.Select(Map);
            _activityIndex.DeleteByType(UintraSearchableTypeEnum.Events);
            _activityIndex.Index(searchableActivities);
        }

        private bool IsCacheable(Event @event) => !IsEventHidden(@event) && IsActualPublishDate(@event);

        private bool IsActualPublishDate(Event @event) => DateTime.Compare(@event.PublishDate, DateTime.Now) <= 0;

        private bool IsEventHidden(Event @event) => @event == null || @event.IsHidden;

        private SearchableUintraActivity Map(Event @event)
        {
            var searchableActivity = @event.Map<SearchableUintraActivity>();
            searchableActivity.Url = _linkService.GetLinks(@event.Id).Details;
            searchableActivity.UserTagNames = _userTagService.Get(@event.Id).Select(t => t.Text);
            return searchableActivity;
        }

        private ActivitySubscribeSettingDto Map(IIntranetActivity activity)
        {
            var @event = (Event) activity;
            return @event.Map<ActivitySubscribeSettingDto>();
        }

        public BroadcastResult Handle(VideoConvertedCommand command)
        {
            var entityId = _intranetMediaService.GetEntityIdByMediaId(command.MediaId);
            var entity = Get(entityId);
            if (entity == null)
            {
                return BroadcastResult.Success;
            }

            entity.ModifyDate = DateTime.Now;
            return BroadcastResult.Success;
        }
    }
}