using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Helpers;
using Compent.uIntra.Core.Search.Entities;
using Compent.uIntra.Core.UserTags.Indexers;
using uIntra.Bulletins;
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
using uIntra.Groups;
using uIntra.Likes;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Search;
using uIntra.Subscribe;
using uIntra.Tagging.UserTags;

namespace Compent.uIntra.Core.Bulletins
{
    public class BulletinsService : BulletinsServiceBase<Bulletin>,
        IBulletinsService<Bulletin>,
        IFeedItemService,
        ICommentableService,
        ILikeableService,
        INotifyableService,
        IIndexer
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly ISubscribeService _subscribeService;
        private readonly IPermissionsService _permissionsService;
        private readonly INotificationsService _notificationService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly IElasticUintraActivityIndex _activityIndex;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly ISearchableTypeProvider _searchableTypeProvider;
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityLinkService _linkService;
        private readonly INotifierDataHelper _notifierDataHelper;
        private readonly UserTagService _userTagService;

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
            IFeedTypeProvider centralFeedTypeProvider,
            IElasticUintraActivityIndex activityIndex,
            IDocumentIndexer documentIndexer,
            ISearchableTypeProvider searchableTypeProvider,
            IMediaHelper mediaHelper,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLinkService linkService,
            INotifierDataHelper notifierDataHelper, UserTagService userTagService)
            : base(intranetActivityRepository, cacheService, activityTypeProvider, intranetMediaService)
        {
            _intranetUserService = intranetUserService;
            _commentsService = commentsService;
            _likesService = likesService;
            _permissionsService = permissionsService;
            _subscribeService = subscribeService;
            _notificationService = notificationService;
            _activityTypeProvider = activityTypeProvider;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _activityIndex = activityIndex;
            _documentIndexer = documentIndexer;
            _searchableTypeProvider = searchableTypeProvider;
            _mediaHelper = mediaHelper;
            _groupActivityService = groupActivityService;
            _linkService = linkService;
            _notifierDataHelper = notifierDataHelper;
            _userTagService = userTagService;
        }


        public override IIntranetType ActivityType => _activityTypeProvider.Get(IntranetActivityTypeEnum.Bulletins.ToInt());


        public MediaSettings GetMediaSettings()
        {
            return _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.BulletinsContent.ToInt());
        }

        protected override void UpdateCache()
        {
            base.UpdateCache();
            FillIndex();
        }

        public override bool CanEdit(IIntranetActivity cached)
        {
            var result = CanPerform(cached, IntranetActivityActionEnum.Edit);
            return result;
        }

        public bool CanDelete(IIntranetActivity cached)
        {
            var result = CanPerform(cached, IntranetActivityActionEnum.Delete);
            return result;
        }

        public bool IsActual(Bulletin activity)
        {
            return base.IsActual(activity) && activity.PublishDate.Date <= DateTime.Now.Date;
        }

        public FeedSettings GetFeedSettings()
        {
            return new FeedSettings
            {
                Type = _centralFeedTypeProvider.Get(CentralFeedTypeEnum.Bulletins.ToInt()),
                Controller = "Bulletins",
                HasPinnedFilter = false,
                HasSubscribersFilter = false,
            };
        }

        public IEnumerable<IFeedItem> GetItems()
        {
            var items = GetOrderedActualItems();
            return items;
        }

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
            }
        }

        protected override Bulletin UpdateCachedEntity(Guid id)
        {
            var cachedBulletin = Get(id);
            var bulletin = base.UpdateCachedEntity(id);
            if (IsBulletinHidden(bulletin))
            {
                _activityIndex.Delete(id);
                _documentIndexer.DeleteFromIndex(cachedBulletin.MediaIds);
                _mediaHelper.DeleteMedia(cachedBulletin.MediaIds);
                return null;
            }

            _activityIndex.Index(Map(bulletin));
            _documentIndexer.Index(bulletin.MediaIds);
            return bulletin;
        }

        public Comment CreateComment(Guid userId, Guid activityId, string text, Guid? parentId)
        {
            var comment = _commentsService.Create(userId, activityId, text, parentId);
            UpdateCachedEntity(comment.ActivityId);
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

        public ILikeable Add(Guid userId, Guid activityId)
        {
            _likesService.Add(userId, activityId);
            return UpdateCachedEntity(activityId);
        }

        public ILikeable Remove(Guid userId, Guid activityId)
        {
            _likesService.Remove(userId, activityId);
            return UpdateCachedEntity(activityId);
        }

        public IEnumerable<LikeModel> GetLikes(Guid activityId)
        {
            return Get(activityId).Likes;
        }

        public void Notify(Guid entityId, IIntranetType notificationType)
        {
            var notifierData = GetNotifierData(entityId, notificationType);
            if (notifierData != null)
            {
                _notificationService.ProcessNotification(notifierData);
            }
        }

        public void FillIndex()
        {
            var activities = GetAll().Where(s => !IsBulletinHidden(s));
            var searchableActivities = activities.Select(Map);

            var searchableType = _searchableTypeProvider.Get(UintraSearchableTypeEnum.Bulletins.ToInt());
            _activityIndex.DeleteByType(searchableType);
            _activityIndex.Index(searchableActivities);
        }

        private NotifierData GetNotifierData(Guid entityId, IIntranetType notificationType)
        {
            var data = new NotifierData
            {
                NotificationType = notificationType,
                ActivityType = ActivityType
            };

            var currentUser = _intranetUserService.GetCurrentUser();

            switch (notificationType.Id)
            {
                case (int)NotificationTypeEnum.ActivityLikeAdded:
                    {
                        var bulletinsEntity = Get(entityId);
                        data.ReceiverIds = bulletinsEntity.OwnerId.ToEnumerableOfOne();
                        data.Value = _notifierDataHelper.GetLikesNotifierDataModel(bulletinsEntity, notificationType, currentUser.Id);
                    }
                    break;

                case (int)NotificationTypeEnum.CommentAdded:
                case (int)NotificationTypeEnum.CommentEdited:
                    {
                        var comment = _commentsService.Get(entityId);
                        var bulletinsEntity = Get(comment.ActivityId);
                        data.ReceiverIds = bulletinsEntity.OwnerId.ToEnumerableOfOne();
                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(bulletinsEntity, comment, notificationType, comment.UserId);
                    }
                    break;

                case (int)NotificationTypeEnum.CommentReplied:
                    {
                        var comment = _commentsService.Get(entityId);
                        var bulletinsEntity = Get(comment.ActivityId);
                        data.ReceiverIds = comment.UserId.ToEnumerableOfOne();
                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(bulletinsEntity, comment, notificationType, currentUser.Id);
                    }
                    break;

                case (int)NotificationTypeEnum.CommentLikeAdded:
                    {
                        var comment = _commentsService.Get(entityId);
                        var bulletinsEntity = Get(comment.ActivityId);
                        data.ReceiverIds = currentUser.Id == comment.UserId
                            ? Enumerable.Empty<Guid>()
                            : comment.UserId.ToEnumerableOfOne();

                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(bulletinsEntity, comment, notificationType, currentUser.Id);
                    }
                    break;

                default:
                    return null;
            }

            return data;
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

        private bool CanPerform(IIntranetActivity cached, IntranetActivityActionEnum action)
        {
            var currentUser = _intranetUserService.GetCurrentUser();

            var isWebmaster = _permissionsService.IsUserWebmaster(currentUser);
            if (isWebmaster)
            {
                return true;
            }

            var ownerId = Get(cached.Id).OwnerId;
            var isOwner = ownerId == currentUser.Id;

            var isUserHasPermissions = _permissionsService.IsRoleHasPermissions(currentUser.Role, ActivityType, action);
            return isOwner && isUserHasPermissions;
        }

        private bool IsBulletinHidden(Bulletin bulletin)
        {
            return bulletin == null || bulletin.IsHidden;
        }

        private SearchableUintraActivity Map(Bulletin bulletin)
        {
            var searchableActivity = bulletin.Map<SearchableUintraActivity>();
            searchableActivity.Url = _linkService.GetLinks(bulletin.Id).Details;
            searchableActivity.UserTagNames = _userTagService.GetRelatedTags(bulletin.Id).Select(t => t.Text);
            return searchableActivity;
        }
    }
}