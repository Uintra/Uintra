using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Helpers;
using Extensions;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Core.Activity;
using uIntra.Core.Caching;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Events;
using uIntra.Groups;
using uIntra.Likes;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Search;
using uIntra.Subscribe;

namespace Compent.uIntra.Core.Events
{
    public class EventsService : IntranetActivityService<Event>,
        IEventsService<Event>,
        IFeedItemService,
        ISubscribableService,
        ILikeableService,
        ICommentableService,
        INotifyableService,
        IReminderableService<Event>,
        IIndexer
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly ISubscribeService _subscribeService;
        private readonly IPermissionsService _permissionsService;
        private readonly INotificationsService _notificationService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IElasticActivityIndex _activityIndex;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly ISearchableTypeProvider _searchableTypeProvider;
        private readonly IActivityLinkService _linkService;
        private readonly INotifierDataHelper _notifierDataHelper;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivitySubscribeSettingService _activitySubscribeSettingService;

        public EventsService(
            IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            ICommentsService commentsService,
            ILikesService likesService,
            ISubscribeService subscribeService,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            IMediaHelper mediaHelper,
            IElasticActivityIndex activityIndex,
            IDocumentIndexer documentIndexer,
            IActivityTypeProvider activityTypeProvider,
            ISearchableTypeProvider searchableTypeProvider,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLinkService linkService,
            INotifierDataHelper notifierDataHelper,
            IActivitySubscribeSettingService activitySubscribeSettingService)
            : base(intranetActivityRepository, cacheService, activityTypeProvider, intranetMediaService)
        {
            _intranetUserService = intranetUserService;
            _commentsService = commentsService;
            _likesService = likesService;
            _subscribeService = subscribeService;
            _permissionsService = permissionsService;
            _notificationService = notificationService;
            _mediaHelper = mediaHelper;
            _activityIndex = activityIndex;
            _documentIndexer = documentIndexer;
            _activityTypeProvider = activityTypeProvider;
            _searchableTypeProvider = searchableTypeProvider;
            _groupActivityService = groupActivityService;
            _linkService = linkService;
            _notifierDataHelper = notifierDataHelper;
            _activitySubscribeSettingService = activitySubscribeSettingService;
        }

        public override IIntranetType ActivityType => _activityTypeProvider.Get(IntranetActivityTypeEnum.Events.ToInt());

        public IEnumerable<Event> GetPastEvents()
        {
            return GetAll().Where(@event => !IsActual(@event) && !@event.IsHidden);
        }

        public IEnumerable<Event> GetComingEvents(DateTime fromDate)
        {
            var events = GetAll()
                .Where(e => e.StartDate > fromDate)
                .OrderBy(e => e.PublishDate);
            return events;
        }

        public void Hide(Guid id)
        {
            var @event = Get(id);
            @event.IsHidden = true;
            Save(@event);
        }

        public bool CanEditSubscribe(Event activity) => !activity.Subscribers.Any();

        public bool CanSubscribe(Guid activityId)
        {
            var @event = Get(activityId);
            return IsActual(@event) && @event.CanSubscribe;
        }

        public MediaSettings GetMediaSettings()
        {
            return _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.EventsContent.ToInt());
        }

        public FeedSettings GetFeedSettings()
        {
            return new FeedSettings
            {
                Type = ActivityType,
                Controller = "Events",
                HasSubscribersFilter = true,
                HasPinnedFilter = true,
            };
        }

        public override bool CanEdit(IIntranetActivity cached)
        {
            var currentUser = _intranetUserService.GetCurrentUser();

            var isWebmaster = _permissionsService.IsUserWebmaster(currentUser);
            if (isWebmaster)
            {
                return true;
            }

            var ownerId = Get(cached.Id).OwnerId;
            var isOwner = ownerId == currentUser.Id;

            var isUserHasPermissions = _permissionsService.IsRoleHasPermissions(currentUser.Role, ActivityType, IntranetActivityActionEnum.Edit);
            return isOwner && isUserHasPermissions;
        }

        public IEnumerable<IFeedItem> GetItems()
        {
            var items = GetOrderedActualItems();
            return items;
        }

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
            base.Save(activity, savedActivity =>
                 {
                     _activitySubscribeSettingService.Save(Map(savedActivity));
                 });
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

        protected override Event UpdateCachedEntity(Guid id)
        {
            var cachedEvent = Get(id);
            var @event = base.UpdateCachedEntity(id);
            if (IsCacheable(@event))
            {
                _activityIndex.Index(Map(@event));
                _documentIndexer.Index(@event.MediaIds);
                return @event;                
            }
            _activityIndex.Delete(id);
            _documentIndexer.DeleteFromIndex(cachedEvent.MediaIds);
            _mediaHelper.DeleteMedia(cachedEvent.MediaIds);
            return null;
        }

        public void UnSubscribe(Guid userId, Guid activityId)
        {
            _subscribeService.Unsubscribe(userId, activityId);
            UpdateCachedEntity(activityId);
        }

        public void UpdateNotification(Guid id, bool value)
        {
            var subscribe = _subscribeService.UpdateNotification(id, value);
            UpdateCachedEntity(subscribe.ActivityId);
        }

        public ILikeable AddLike(Guid userId, Guid activityId)
        {
            _likesService.Add(userId, activityId);
            return UpdateCachedEntity(activityId);
        }

        public ILikeable RemoveLike(Guid userId, Guid activityId)
        {
            _likesService.Remove(userId, activityId);
            return UpdateCachedEntity(activityId);
        }

        public IEnumerable<LikeModel> GetLikes(Guid activityId)
        {
            return Get(activityId).Likes;
        }

        public Comment CreateComment(Guid userId, Guid activityId, string text, Guid? parentId)
        {
            var comment = _commentsService.Create(userId, activityId, text, parentId);
            UpdateCachedEntity(activityId);
            return comment;
        }

        public void UpdateComment(Guid id, string text)
        {
            var comment = _commentsService.Update(id, text);
            UpdateCachedEntity(comment.ActivityId);
        }

        public void DeleteComment(Guid id)
        {
            var comment = _commentsService.Get(id);
            _commentsService.Delete(id);
            UpdateCachedEntity(comment.ActivityId);
        }

        public ICommentable GetCommentsInfo(Guid activityId)
        {
            return Get(activityId);
        }

        public bool CanEditSubscribe(Guid activityId)
        {
            return !Get(activityId).Subscribers.Any();
        }

        public void Notify(Guid entityId, IIntranetType notificationType)
        {
            var notifierData = GetNotifierData(entityId, notificationType);

            if (notifierData != null)
            {
                _notificationService.ProcessNotification(notifierData);
            }
        }

        private NotifierData GetNotifierData(Guid entityId, IIntranetType notificationType)
        {
            var currentUser = _intranetUserService.GetCurrentUser();

            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = ActivityType

            };

            switch (notificationType.Id)
            {
                case (int)NotificationTypeEnum.CommentReplied:
                    {
                        var comment = _commentsService.Get(entityId);
                        var currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = comment.UserId.ToEnumerable();
                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(currentEvent, comment, notificationType, currentUser.Id);
                    }
                    break;
                case (int)NotificationTypeEnum.CommentEdited:
                    {
                        var comment = _commentsService.Get(entityId);
                        var currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = currentEvent.OwnerId.ToEnumerable();
                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(currentEvent, comment, notificationType, comment.UserId);

                    }
                    break;
                case (int)NotificationTypeEnum.CommentAdded:
                    {
                        var comment = _commentsService.Get(entityId);
                        var currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = GetNotifiedSubscribers(currentEvent).Concat(currentEvent.OwnerId.ToEnumerable()).Distinct();
                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(currentEvent, comment, notificationType, comment.UserId);
                    }
                    break;
                case (int)NotificationTypeEnum.ActivityLikeAdded:
                    {
                        var currentEvent = Get(entityId);
                        data.ReceiverIds = currentEvent.OwnerId.ToEnumerable();
                        data.Value = _notifierDataHelper.GetLikesNotifierDataModel(currentEvent, notificationType, currentUser.Id);
                    }
                    break;
                case (int)NotificationTypeEnum.CommentLikeAdded:
                    {
                        var comment = _commentsService.Get(entityId);
                        var currentEvent = Get(comment.ActivityId);
                        data.ReceiverIds = currentUser.Id == comment.UserId
                            ? Enumerable.Empty<Guid>()
                        : comment.UserId.ToEnumerable();

                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(currentEvent, comment, notificationType, currentUser.Id);
                    }
                    break;

                case (int)NotificationTypeEnum.BeforeStart:
                    {
                        var currentEvent = Get(entityId);
                        data.ReceiverIds = GetNotifiedSubscribers(currentEvent);
                        data.Value = _notifierDataHelper.GetActivityReminderDataModel(currentEvent, notificationType);
                    }
                    break;

                case (int)NotificationTypeEnum.EventHided:
                case (int)NotificationTypeEnum.EventUpdated:
                    {
                        var currentEvent = Get(entityId);
                        data.ReceiverIds = GetNotifiedSubscribers(currentEvent);
                        data.Value = _notifierDataHelper.GetActivityNotifierDataModel(currentEvent, notificationType, currentUser.Id);
                    }
                    break;
                default:
                    return null;
            }
            return data;
        }

        private static IEnumerable<Guid> GetNotifiedSubscribers(Event currentEvent)
        {
            return currentEvent.Subscribers.Where(s => !s.IsNotificationDisabled).Select(s => s.UserId);
        }

        public ISubscribable Subscribe(Guid userId, Guid activityId)
        {
            _subscribeService.Subscribe(userId, activityId);
            return UpdateCachedEntity(activityId);
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

            var searchableType = _searchableTypeProvider.Get(SearchableTypeEnum.Events.ToInt());
            _activityIndex.DeleteByType(searchableType);
            _activityIndex.Index(searchableActivities);
        }

        private bool IsCacheable(Event @event)
        {
            return !IsEventHidden(@event) && IsActualPublishDate(@event);
        }

        private bool IsActualPublishDate(Event @event)
        {
            return DateTime.Compare(@event.PublishDate, DateTime.Now) <= 0;
        }

        private bool IsEventHidden(Event @event)
        {
            return @event == null || @event.IsHidden;
        }

        private SearchableActivity Map(Event @event)
        {
            var searchableActivity = @event.Map<SearchableActivity>();
            searchableActivity.Url = _linkService.GetLinks(@event.Id).Details;
            return searchableActivity;
        }

        private ActivitySubscribeSettingDto Map(IIntranetActivity activity)
        {
            var @event = (Event)activity;
            return @event.Map<ActivitySubscribeSettingDto>();
        }
    }
}