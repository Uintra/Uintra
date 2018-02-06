using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Helpers;
using Compent.uIntra.Core.Search.Entities;
using Compent.uIntra.Core.UserTags.Indexers;
using Extensions;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Caching;
using uIntra.Core.Extensions;
using uIntra.Core.Links;
using uIntra.Core.Location;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Groups;
using uIntra.Likes;
using uIntra.News;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Search;
using uIntra.Subscribe;
using uIntra.Tagging.UserTags;

namespace Compent.uIntra.Core.News
{
    public class NewsService : NewsServiceBase<Entities.News>,
        INewsService<Entities.News>,
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
        private readonly IMediaHelper _mediaHelper;
        private readonly IElasticUintraActivityIndex _activityIndex;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly ISearchableTypeProvider _searchableTypeProvider;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityLinkService _linkService;
        private readonly INotifierDataHelper _notifierDataHelper;
        private readonly IUserTagService _userTagService;
        private readonly IActivityLocationService _activityLocationService;

        public NewsService(IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            ICommentsService commentsService,
            ILikesService likesService,
            ISubscribeService subscribeService,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            IMediaHelper mediaHelper,
            IElasticUintraActivityIndex activityIndex,
            IDocumentIndexer documentIndexer,
            IActivityTypeProvider activityTypeProvider,
            IFeedTypeProvider centralFeedTypeProvider,
            ISearchableTypeProvider searchableTypeProvider,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IIntranetMediaService intranetMediaService,
            IGroupActivityService groupActivityService,
            IActivityLinkService linkService,
            INotifierDataHelper notifierDataHelper,
            IActivityLocationService activityLocationService,
            IUserTagService userTagService)
            : base(intranetActivityRepository, cacheService, intranetUserService, activityTypeProvider, intranetMediaService, activityLocationService)
        {
            _intranetUserService = intranetUserService;
            _commentsService = commentsService;
            _likesService = likesService;
            _permissionsService = permissionsService;
            _subscribeService = subscribeService;
            _notificationService = notificationService;
            _mediaHelper = mediaHelper;
            _activityIndex = activityIndex;
            _documentIndexer = documentIndexer;
            _activityTypeProvider = activityTypeProvider;
            _centralFeedTypeProvider = centralFeedTypeProvider;
            _searchableTypeProvider = searchableTypeProvider;
            _documentTypeAliasProvider = documentTypeAliasProvider;
            _groupActivityService = groupActivityService;
            _linkService = linkService;
            _notifierDataHelper = notifierDataHelper;
            _userTagService = userTagService;
            _activityLocationService = activityLocationService;
        }

        protected List<string> OverviewXPath => new List<string> { _documentTypeAliasProvider.GetHomePage(), _documentTypeAliasProvider.GetOverviewPage(ActivityType) };
        public override IIntranetType ActivityType => _activityTypeProvider.Get((int)IntranetActivityTypeEnum.News);

        public MediaSettings GetMediaSettings()
        {
            return _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.NewsContent.ToInt());
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

        public FeedSettings GetFeedSettings()
        {
            return new FeedSettings
            {
                Type = _centralFeedTypeProvider.Get(CentralFeedTypeEnum.News.ToInt()),
                Controller = "News",
                HasSubscribersFilter = false,
                HasPinnedFilter = true,
            };
        }

        public IEnumerable<IFeedItem> GetItems()
        {
            var items = GetOrderedActualItems();
            return items;
        }

        private IOrderedEnumerable<Entities.News> GetOrderedActualItems() =>
            GetManyActual().OrderByDescending(i => i.PublishDate);

        protected override void MapBeforeCache(IList<Entities.News> cached)
        {
            foreach (var activity in cached)
            {
                var entity = activity;
                entity.Location = _activityLocationService.Get(entity.Id);
                entity.GroupId = _groupActivityService.GetGroupId(activity.Id);
                _subscribeService.FillSubscribers(entity);
                _commentsService.FillComments(entity);
                _likesService.FillLikes(entity);
            }
        }

        protected override void UpdateCache()
        {
            base.UpdateCache();
            FillIndex();
        }

        protected override Entities.News UpdateCachedEntity(Guid id)
        {
            var cachedNews = Get(id);
            var news = base.UpdateCachedEntity(id);
            if (IsInCache(news))
            {
                _activityIndex.Index(Map(news));
                _documentIndexer.Index(news.MediaIds);
                return news;
            }

            if (cachedNews == null) return null;

            _activityIndex.Delete(id);
            _documentIndexer.DeleteFromIndex(cachedNews.MediaIds);
            _mediaHelper.DeleteMedia(cachedNews.MediaIds);
            return null;
        }

        public CommentModel CreateComment(CommentCreateDto dto)
        {
            var comment = _commentsService.Create(dto);
            UpdateCachedEntity(comment.ActivityId);
            return comment;
        }

        public void UpdateComment(CommentEditDto dto)
        {
            var comment = _commentsService.Update(dto);
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
            var activities = GetAll().Where(IsInCache);
            var searchableActivities = activities.Select(Map);

            var seachableType = _searchableTypeProvider.Get(UintraSearchableTypeEnum.News.ToInt());
            _activityIndex.DeleteByType(seachableType);
            _activityIndex.Index(searchableActivities);
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
                case (int)NotificationTypeEnum.ActivityLikeAdded:
                    {
                        var news = Get(entityId);
                        data.ReceiverIds = news.OwnerId.ToEnumerable();
                        data.Value = _notifierDataHelper.GetLikesNotifierDataModel(news, notificationType, currentUser.Id);
                    }
                    break;

                case (int)NotificationTypeEnum.CommentLikeAdded:
                    {
                        var comment = _commentsService.Get(entityId);
                        var news = Get(comment.ActivityId);
                        data.ReceiverIds = currentUser.Id == comment.UserId
                            ? Enumerable.Empty<Guid>()
                            : comment.UserId.ToEnumerable();

                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(news, comment, notificationType, currentUser.Id);
                    }
                    break;

                case (int)NotificationTypeEnum.CommentAdded:
                case (int)NotificationTypeEnum.CommentEdited:
                    {
                        var comment = _commentsService.Get(entityId);
                        var news = Get(comment.ActivityId);
                        data.ReceiverIds = news.OwnerId.ToEnumerable();
                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(news, comment, notificationType, comment.UserId);
                    }
                    break;

                case (int)NotificationTypeEnum.CommentReplied:
                    {
                        var comment = _commentsService.Get(entityId);
                        var news = Get(comment.ActivityId);
                        data.ReceiverIds = comment.UserId.ToEnumerable();
                        data.Value = _notifierDataHelper.GetCommentNotifierDataModel(news, comment, notificationType, currentUser.Id);
                    }
                    break;

                default:
                    return null;
            }
            return data;
        }

        private bool IsInCache(Entities.News news)
        {
            return !IsNewsHidden(news) && IsActualPublishDate(news);
        }

        private bool IsNewsHidden(Entities.News news)
        {
            return news == null || news.IsHidden;
        }

        private bool IsActualPublishDate(Entities.News news)
        {
            return DateTime.Compare(news.PublishDate, DateTime.Now) <= 0;
        }

        private SearchableUintraActivity Map(Entities.News news)
        {
            var searchableActivity = news.Map<SearchableUintraActivity>();
            searchableActivity.Url = _linkService.GetLinks(news.Id).Details;
            searchableActivity.UserTagNames = _userTagService.Get(news.Id).Select(t => t.Text);
            return searchableActivity;
        }
    }
}