using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Extensions;
using Compent.Uintra.Core.Helpers;
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
using Uintra.Notification.Configuration;
using Uintra.Search;
using Uintra.Subscribe;
using Uintra.Tagging.UserTags;

namespace Compent.Uintra.Core.Bulletins
{
    public class BulletinsService : BulletinsServiceBase<Bulletin>,
        IBulletinsService<Bulletin>,
        IFeedItemService,
        INotifyableService,
        IIndexer
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly ISubscribeService _subscribeService;
        private readonly IPermissionsService _permissionsService;
        private readonly INotificationsService _notificationService;
        private readonly IElasticUintraActivityIndex _activityIndex;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityLinkService _linkService;
        private readonly INotifierDataHelper _notifierDataHelper;
        private readonly IUserTagService _userTagService;
        private readonly IActivityLinkPreviewService _activityLinkPreviewService;
        private readonly IGroupService _groupService;

        public BulletinsService(
            IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            ICommentsService commentsService,
            ILikesService likesService,
            ISubscribeService subscribeService,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            IActivityTypeProvider activityTypeProvider,
            IElasticUintraActivityIndex activityIndex,
            IDocumentIndexer documentIndexer,
            IMediaHelper mediaHelper,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLinkService linkService,
            INotifierDataHelper notifierDataHelper,
            IActivityLocationService activityLocationService,
            IUserTagService userTagService,
            IActivityLinkPreviewService activityLinkPreviewService,
            IGroupService groupService)
            : base(intranetActivityRepository, cacheService, activityTypeProvider, intranetMediaService, activityLocationService,activityLinkPreviewService)
        {
            _intranetUserService = intranetUserService;
            _commentsService = commentsService;
            _likesService = likesService;
            _permissionsService = permissionsService;
            _subscribeService = subscribeService;
            _notificationService = notificationService;
            _activityIndex = activityIndex;
            _documentIndexer = documentIndexer;
            _mediaHelper = mediaHelper;
            _groupActivityService = groupActivityService;
            _linkService = linkService;
            _notifierDataHelper = notifierDataHelper;
            _userTagService = userTagService;
            _activityLinkPreviewService = activityLinkPreviewService;
            _groupService = groupService;
        }

        public override Enum Type => IntranetActivityTypeEnum.Bulletins;

        public MediaSettings GetMediaSettings() => _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.BulletinsContent);

        protected override void UpdateCache()
        {
            base.UpdateCache();
            FillIndex();
        }

        public override bool CanEdit(IIntranetActivity activity) => CanPerform(activity, IntranetActivityActionEnum.Edit);

        public bool CanDelete(IIntranetActivity cached) => CanPerform(cached, IntranetActivityActionEnum.Delete);

        public bool IsActual(Bulletin activity) =>
            base.IsActual(activity) && activity.PublishDate.Date <= DateTime.Now.Date;

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
                _subscribeService.FillSubscribers(entity);
                _commentsService.FillComments(entity);
                _likesService.FillLikes(entity);
                FillLinkPreview(entity);
            }
        }

        [Obsolete("This method should be removed. Use UpdateActivityCache instead.")]
        protected override Bulletin UpdateCachedEntity(Guid id) => UpdateActivityCache(id);

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
            var notifierData = GetNotifierData(entityId, notificationType);
            if (notifierData != null)
            {
                _notificationService.ProcessNotification(notifierData);
            }
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

        private NotifierData GetNotifierData(Guid entityId, Enum notificationType)
        {
            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = Type
            };

            var currentUser = _intranetUserService.GetCurrentUser();

            switch (notificationType)
            {
                case NotificationTypeEnum.ActivityLikeAdded:
                    {
                        var bulletinsEntity = Get(entityId);
                        data.ReceiverIds = bulletinsEntity.OwnerId.ToEnumerable();
                        data.Value = _notifierDataHelper.GetLikesNotifierDataModel(bulletinsEntity, notificationType, currentUser.Id);
                    }
                    break;

                case NotificationTypeEnum.CommentAdded:
                case NotificationTypeEnum.CommentEdited:
                    {
                        var comment = _commentsService.Get(entityId);
                        var bulletinsEntity = Get(comment.ActivityId);
                        data.ReceiverIds = bulletinsEntity.OwnerId.ToEnumerable();
                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(bulletinsEntity, comment, notificationType, comment.UserId);
                    }
                    break;

                case NotificationTypeEnum.CommentReplied:
                    {
                        var comment = _commentsService.Get(entityId);
                        var bulletinsEntity = Get(comment.ActivityId);
                        data.ReceiverIds = comment.UserId.ToEnumerable();
                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(bulletinsEntity, comment, notificationType, currentUser.Id);
                    }
                    break;

                case NotificationTypeEnum.CommentLikeAdded:
                    {
                        var comment = _commentsService.Get(entityId);
                        var bulletinsEntity = Get(comment.ActivityId);
                        data.ReceiverIds = currentUser.Id == comment.UserId
                            ? Enumerable.Empty<Guid>()
                            : comment.UserId.ToEnumerable();

                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(bulletinsEntity, comment, notificationType, currentUser.Id);
                    }
                    break;

                default:
                    return null;
            }

            return data;
        }

        private bool CanPerform(IIntranetActivity cached, IntranetActivityActionEnum action)
        {
            var currentUser = _intranetUserService.GetCurrentUser();

            var isWebmaster = _permissionsService.IsUserWebmaster(currentUser);
            if (isWebmaster) return true;

            var ownerId = Get(cached.Id).OwnerId;
            var isOwner = ownerId == currentUser.Id;

            var isUserHasPermissions = _permissionsService.IsRoleHasPermissions(currentUser.Role, Type, action);
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
    }
}