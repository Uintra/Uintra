using System;
using System.Collections.Generic;
using System.Linq;
using Compent.CommandBus;
using Compent.Extensions;
using Compent.Uintra.Core.Helpers;
using Compent.Uintra.Core.Notification;
using Compent.Uintra.Core.Search.Entities;
using Compent.Uintra.Core.UserTags.Indexers;
using Uintra.Bulletins;
using Uintra.CentralFeed;
using Uintra.Comments;
using Uintra.Core.Activity;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.LinkPreview;
using Uintra.Core.Links;
using Uintra.Core.Location;
using Uintra.Core.Media;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Core.User.Permissions;
using Uintra.Groups;
using Uintra.Likes;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Search;
using Uintra.Subscribe;
using Uintra.Tagging.UserTags;
using static Uintra.Notification.Configuration.NotificationTypeEnum;

namespace Compent.Uintra.Core.Bulletins
{
    public class BulletinsService : BulletinsServiceBase<Bulletin>,
        IBulletinsService<Bulletin>,
        IFeedItemService,
        INotifyableService,
        IIndexer,
        IHandle<VideoConvertedCommand>
    {
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly ISubscribeService _subscribeService;
        private readonly IOldPermissionsService _oldPermissionsService;
        private readonly INotificationsService _notificationService;
        private readonly IElasticUintraActivityIndex _activityIndex;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMediaService _intranetMediaService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityLinkService _linkService;
        private readonly IUserTagService _userTagService;
        private readonly IActivityLinkPreviewService _activityLinkPreviewService;
        private readonly IGroupService _groupService;
        private readonly INotifierDataBuilder _notifierDataBuilder;

        public BulletinsService(
            IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            ICommentsService commentsService,
            ILikesService likesService,
            ISubscribeService subscribeService,
            IOldPermissionsService oldPermissionsService,
            INotificationsService notificationService,
            IActivityTypeProvider activityTypeProvider,
            IElasticUintraActivityIndex activityIndex,
            IDocumentIndexer documentIndexer,
            IMediaHelper mediaHelper,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLinkService linkService,
            IActivityLocationService activityLocationService,
            IUserTagService userTagService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IGroupService groupService,
            INotifierDataBuilder notifierDataBuilder)
            : base(intranetActivityRepository, cacheService, activityTypeProvider, intranetMediaService,
                activityLocationService, activityLinkPreviewService)
        {
            _intranetMemberService = intranetMemberService;
            _commentsService = commentsService;
            _likesService = likesService;
            _oldPermissionsService = oldPermissionsService;
            _subscribeService = subscribeService;
            _notificationService = notificationService;
            _activityIndex = activityIndex;
            _documentIndexer = documentIndexer;
            _mediaHelper = mediaHelper;
            _intranetMediaService = intranetMediaService;
            _groupActivityService = groupActivityService;
            _linkService = linkService;
            _userTagService = userTagService;
            _activityLinkPreviewService = activityLinkPreviewService;
            _groupService = groupService;
            _notifierDataBuilder = notifierDataBuilder;
        }

        public override Enum Type => IntranetActivityTypeEnum.Bulletins;

        public MediaSettings GetMediaSettings() => _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.BulletinsContent);

        protected override void UpdateCache()
        {
            base.UpdateCache();
            FillIndex();
        }

        public override bool CanEdit(IIntranetActivity activity) => CanPerform(activity, IntranetActionEnum.Edit);

        public bool CanDelete(IIntranetActivity cached) => CanPerform(cached, IntranetActionEnum.Delete);

        public FeedSettings GetFeedSettings()
        {
            return new FeedSettings
            {
                Type = CentralFeedTypeEnum.Bulletins,
                Controller = "Bulletins",
                HasPinnedFilter = false,
                HasSubscribersFilter = false,
            };
        }

        public IEnumerable<IFeedItem> GetItems() => GetOrderedActualItems();

        private IOrderedEnumerable<Bulletin> GetOrderedActualItems() =>
            GetManyActual().OrderByDescending(i => i.PublishDate);

        protected override void MapBeforeCache(IList<Bulletin> cached)
        {
            foreach (var activity in cached)
            {
                var entity = activity;
                entity.GroupId = _groupActivityService.GetGroupId(activity.Id);
                _commentsService.FillComments(entity);
                _likesService.FillLikes(entity);
                FillLinkPreview(entity);
            }
        }

        public override Bulletin UpdateActivityCache(Guid id)
        {
            var cachedBulletin = Get(id);
            var bulletin = base.UpdateActivityCache(id);
            if (IsCacheable(bulletin) && (bulletin.GroupId is null || _groupService.IsActivityFromActiveGroup(bulletin)))
            {
                _activityIndex.Index(Map(bulletin));
                _documentIndexer.Index(bulletin.MediaIds);
                return bulletin;
            }

            if (cachedBulletin == null) return null;

            _activityIndex.Delete(id);
            _documentIndexer.DeleteFromIndex(cachedBulletin.MediaIds);
            _mediaHelper.DeleteMedia(cachedBulletin.MediaIds);
            return null;
        }

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

        public void FillIndex()
        {
            var activities = GetAll().Where(IsCacheable);
            var searchableActivities = activities.Select(Map);
            _activityIndex.DeleteByType(UintraSearchableTypeEnum.Bulletins);
            _activityIndex.Index(searchableActivities);
        }

        private void FillLinkPreview(Bulletin bulletin)
        {
            var linkPreview = _activityLinkPreviewService.GetActivityLinkPreview(bulletin.Id);
            bulletin.LinkPreview = linkPreview;
            bulletin.LinkPreviewId = linkPreview?.Id;
        }

        private bool CanPerform(IIntranetActivity cached, IntranetActionEnum action)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            var isWebmaster = _oldPermissionsService.IsUserWebmaster(currentMember);
            if (isWebmaster) return true;

            var ownerId = Get(cached.Id).OwnerId;
            var isOwner = ownerId == currentMember.Id;

            var isUserHasPermissions = _oldPermissionsService.IsRoleHasPermissions(currentMember.Role, Type, action);
            return isOwner && isUserHasPermissions;
        }

        private bool IsBulletinHidden(Bulletin bulletin) => bulletin == null || bulletin.IsHidden;

        private bool IsCacheable(Bulletin bulletin) =>
            !IsBulletinHidden(bulletin) && IsActualPublishDate(bulletin);

        private bool IsActualPublishDate(Bulletin bulletin) =>
            DateTime.Compare(bulletin.PublishDate, DateTime.Now) <= 0;

        private SearchableUintraActivity Map(Bulletin bulletin)
        {
            var searchableActivity = bulletin.Map<SearchableUintraActivity>();
            searchableActivity.Url = _linkService.GetLinks(bulletin.Id).Details;
            searchableActivity.UserTagNames = _userTagService.Get(bulletin.Id).Select(t => t.Text);
            return searchableActivity;
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