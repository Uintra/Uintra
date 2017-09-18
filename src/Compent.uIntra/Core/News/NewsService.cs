using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.CentralFeed;
using uIntra.Comments;
using uIntra.Core;
using uIntra.Core.Activity;
using uIntra.Core.Caching;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
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
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uIntra.Core.News
{
    public class NewsService : NewsServiceBase<Entities.News>,
        INewsService<Entities.News>,
        ICentralFeedItemService,
        IGroupFeedItemService,
        ICommentableService,
        ILikeableService,
        INotifyableService,
        IIndexer
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISubscribeService _subscribeService;
        private readonly IPermissionsService _permissionsService;
        private readonly INotificationsService _notificationService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IElasticActivityIndex _activityIndex;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IFeedTypeProvider _centralFeedTypeProvider;
        private readonly ISearchableTypeProvider _searchableTypeProvider;
        private readonly IDocumentTypeAliasProvider _documentTypeAliasProvider;
        private readonly IGroupService _groupService;


        public NewsService(IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            ICommentsService commentsService,
            ILikesService likesService,
            ISubscribeService subscribeService,
            UmbracoHelper umbracoHelper,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            IMediaHelper mediaHelper,
            IElasticActivityIndex activityIndex,
            IDocumentIndexer documentIndexer,
            IActivityTypeProvider activityTypeProvider,
            IFeedTypeProvider centralFeedTypeProvider,
            ISearchableTypeProvider searchableTypeProvider,
            IDocumentTypeAliasProvider documentTypeAliasProvider,
            IIntranetMediaService intranetMediaService,
            IIntranetUserContentHelper intranetUserContentHelper,
            IGroupService groupService)
            : base(intranetActivityRepository, cacheService, intranetUserService, activityTypeProvider, intranetMediaService, intranetUserContentHelper)
        {
            _intranetUserService = intranetUserService;
            _commentsService = commentsService;
            _likesService = likesService;
            _umbracoHelper = umbracoHelper;
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
            _groupService = groupService;
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

            var creatorId = Get(cached.Id).CreatorId;
            var isCreator = creatorId == currentUser.Id;

            var isUserHasPermissions = _permissionsService.IsRoleHasPermissions(currentUser.Role, ActivityType, IntranetActivityActionEnum.Edit);
            return isCreator && isUserHasPermissions;
        }

        public FeedSettings GetCentralFeedSettings()
        {
            return new FeedSettings
            {
                Type = _centralFeedTypeProvider.Get(CentralFeedTypeEnum.News.ToInt()),
                Controller = "News",
                //OverviewPage = GetOverviewPage(), // TODO
                //CreatePage = GetCreatePage(),
                HasSubscribersFilter = false,
                HasPinnedFilter = true
            };
        }

        public IFeedItem GetItem(Guid activityId)
        {
            var news = Get(activityId);

            // TODO : checking if activity is assigned to any group. Ask about expected behavior
            if (!news.GroupIds.Any())
                throw new UnauthorizedAccessException("It is a group activity.");

            return news;
        }

        public IFeedItem GetItem(Guid activityId, Guid groupId)
        {
            var news = Get(activityId);

            if (!news.GroupIds.Contains(groupId))
                throw new UnauthorizedAccessException("This activity has no relations with the given group.");

            return news;
        }

        public IEnumerable<IFeedItem> GetItems()
        {
            var items = GetOrderedActualItems().Where(i => !i.GroupIds.Any());
            return items;
        }

        public IEnumerable<IFeedItem> GetItems(Guid groupId)
        {
            var items = GetOrderedActualItems().Where(i => i.GroupIds.Contains(groupId));
            return items;
        }

        private IOrderedEnumerable<Entities.News> GetOrderedActualItems() => 
            GetManyActual().OrderByDescending(i => i.PublishDate);

        protected override void MapBeforeCache(IList<IIntranetActivity> cached)
        {
            foreach (var activity in cached)
            {
                var entity = (Entities.News)activity;
                entity.GroupIds = _groupService.GetGroupIds(activity.Id);
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
            if (IsNewsHidden(news))
            {
                _activityIndex.Delete(id);
                _documentIndexer.DeleteFromIndex(cachedNews.MediaIds);
                _mediaHelper.DeleteMedia(cachedNews.MediaIds);
                return null;
            }

            _activityIndex.Index(Map(news));
            _documentIndexer.Index(news.MediaIds);
            return news;
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
            var activities = GetAll().Where(s => !IsNewsHidden(s));
            var searchableActivities = activities.Select(Map);

            var seachableType = _searchableTypeProvider.Get(SearchableTypeEnum.News.ToInt());
            _activityIndex.DeleteByType(seachableType);
            _activityIndex.Index(searchableActivities);
        }

        private NotifierData GetNotifierData(Guid entityId, IIntranetType notificationType)
        {
            Entities.News news;
            var currentUser = _intranetUserService.GetCurrentUser();
            var data = new NotifierData
            {
                NotificationType = notificationType
            };

            switch (notificationType.Id)
            {
                case (int)NotificationTypeEnum.ActivityLikeAdded:
                    {
                        news = Get(entityId);
                        data.ReceiverIds = news.CreatorId.ToEnumerableOfOne();
                        data.Value = new LikesNotifierDataModel()
                        {
                            //Url = GetDetailsPage().Url.UrlWithQueryString("id", news.Id),
                            Title = news.Title,
                            ActivityType = ActivityType,
                            NotifierId = currentUser.Id,
                            CreatedDate = DateTime.Now
                        };
                    }
                    break;
                case (int)NotificationTypeEnum.CommentLikeAdded:
                    {
                        var comment = _commentsService.Get(entityId);
                        news = Get(comment.ActivityId);

                        data.ReceiverIds = currentUser.Id == comment.UserId
                            ? Enumerable.Empty<Guid>()
                            : comment.UserId.ToEnumerableOfOne();

                        data.Value = new CommentNotifierDataModel
                        {
                            CommentId = entityId,
                            ActivityType = ActivityType,
                            NotifierId = currentUser.Id,
                            Title = news.Title,
                            //Url = GetUrlWithComment(news.Id, comment.Id)
                        };
                    }
                    break;

                case (int)NotificationTypeEnum.CommentAdded:
                case (int)NotificationTypeEnum.CommentEdited:
                    {
                        var comment = _commentsService.Get(entityId);
                        news = Get(comment.ActivityId);
                        data.ReceiverIds = news.CreatorId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel()
                        {
                            ActivityType = ActivityType,
                            NotifierId = comment.UserId,
                            Title = news.Title,
                            //Url = GetUrlWithComment(news.Id, comment.Id)
                        };
                    }
                    break;
                case (int)NotificationTypeEnum.CommentReplied:
                    {
                        var comment = _commentsService.Get(entityId);
                        news = Get(comment.ActivityId);
                        data.ReceiverIds = comment.UserId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel
                        {
                            ActivityType = ActivityType,
                            NotifierId = currentUser.Id,
                            Title = news.Title,
                            //Url = GetUrlWithComment(news.Id, comment.Id),
                            CommentId = comment.Id
                        };
                    }
                    break;
                default:
                    return null;
            }
            return data;
        }

        //private string GetUrlWithComment(Guid newsId, Guid commentId)
        //{
        //    return $"{GetDetailsPage().Url.UrlWithQueryString("id", newsId)}#{_commentsService.GetCommentViewId(commentId)}";
        //}

        private string[] GetPath(params string[] aliases)
        {
            var basePath = new List<string>(OverviewXPath);

            if (aliases.Any())
            {
                basePath.AddRange(aliases.ToList());
            }
            return basePath.ToArray();
        }

        private bool IsNewsHidden(Entities.News news)
        {
            return news == null || news.IsHidden;
        }

        private SearchableActivity Map(Entities.News news)
        {
            var searchableActivity = news.Map<SearchableActivity>();
            //searchableActivity.Url = GetDetailsPage().Url.AddIdParameter(news.Id); TODO
            return searchableActivity;
        }
    }
}