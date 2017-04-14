using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.CentralFeed;
using uCommunity.CentralFeed.Entities;
using uCommunity.Comments;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Caching;
using uCommunity.Core.Extentions;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions;
using uCommunity.Likes;
using uCommunity.News;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Entities;
using uCommunity.Notification.Core.Services;
using uCommunity.Subscribe;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uCommunity.Core
{
    public class NewsService : IntranetActivityItemServiceBase<NewsBase, News.Entities.News>,
        INewsService<NewsBase, News.Entities.News>,
        ICentralFeedItemService,
        ICommentableService,
        ILikeableService,
        INotifyableService
    {
        private readonly IIntranetUserService _intranetUserService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISubscribeService _subscribeService;
        private readonly IPermissionsService _permissionsService;
        private readonly INotificationsService _notificationService;

        public NewsService(IIntranetActivityService intranetActivityService,
            IMemoryCacheService memoryCacheService,
            IIntranetUserService intranetUserService,
            ICommentsService commentsService,
            ILikesService likesService,
            ISubscribeService subscribeService,
            UmbracoHelper umbracoHelper, IPermissionsService permissionsService,
            INotificationsService notificationService)
            : base(intranetActivityService, memoryCacheService)
        {
            _intranetUserService = intranetUserService;
            _commentsService = commentsService;
            _likesService = likesService;
            _umbracoHelper = umbracoHelper;
            this._permissionsService = permissionsService;
            _subscribeService = subscribeService;
            _notificationService = notificationService;
        }

        public MediaSettings GetMediaSettings()
        {
            return new MediaSettings
            {
                MediaRootId = 1099
            };
        }

        public override IPublishedContent GetOverviewPage()
        {
            return _umbracoHelper.TypedContent(1092);
        }

        public override IPublishedContent GetDetailsPage()
        {
            return _umbracoHelper.TypedContent(1094);
        }

        public override IPublishedContent GetCreatePage()
        {
            return _umbracoHelper.TypedContent(1093);
        }

        public override IPublishedContent GetEditPage()
        {
            return _umbracoHelper.TypedContent(1095);
        }

        protected override List<string> OverviewXPath { get; }


        public override IntranetActivityTypeEnum ActivityType => IntranetActivityTypeEnum.News;


        public override bool CanEdit(NewsBase activity)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var isAllowed = _permissionsService.IsRoleHasPermissions(currentUser.Role, IntranetActivityTypeEnum.News, IntranetActivityActionEnum.Edit);
            return isAllowed;
        }

        public CentralFeedSettings GetCentralFeedSettings()
        {
            return new CentralFeedSettings
            {
                Type = ActivityType,
                Controller = "News",
                OverviewPage = GetOverviewPage(),
                CreatePage = GetCreatePage()
            };
        }

        public bool IsActual(News.Entities.News activity)
        {
            return base.IsActual(activity) && activity.PublishDate.Date <= DateTime.Now.Date;
        }

        public ICentralFeedItem GetItem(Guid activityId)
        {
            var news = Get(activityId);
            return news;
        }

        public IEnumerable<ICentralFeedItem> GetItems()
        {
            var items = GetManyActual().OrderByDescending(i => i.PublishDate);
            return items;
        }

        protected override News.Entities.News FillPropertiesOnGet(IntranetActivityEntity entity)
        {
            var activity = base.FillPropertiesOnGet(entity);
            _subscribeService.FillSubscribers(activity);
            _intranetUserService.FillCreator(activity);
            _commentsService.FillComments(activity);
            _likesService.FillLikes(activity);

            return activity;
        }

        public void CreateComment(Guid userId, Guid activityId, string text, Guid? parentId)
        {
            var comment = _commentsService.Create(userId, activityId, text, parentId);
            FillCache(activityId);
            Notify(parentId ?? comment.Id, parentId.HasValue ? NotificationTypeEnum.CommentReplyed: NotificationTypeEnum.CommentAdded);
        }

        public void UpdateComment(Guid id, string text)
        {
            var comment = _commentsService.Update(id, text);
            FillCache(comment.ActivityId);
            Notify(comment.Id, NotificationTypeEnum.CommentEdited);
        }

        public void DeleteComment(Guid id)
        {
            var comment = _commentsService.Get(id);
            _commentsService.Delete(id);
            FillCache(comment.ActivityId);
        }

        public ICommentable GetCommentsInfo(Guid activityId)
        {
            return Get(activityId);
        }

        public ILikeable Add(Guid userId, Guid activityId)
        {
            _likesService.Add(userId, activityId);
            Notify(activityId, NotificationTypeEnum.LikeAdded);
            return FillCache(activityId);
        }

        public ILikeable Remove(Guid userId, Guid activityId)
        {
            _likesService.Remove(userId, activityId);
            return FillCache(activityId);
        }

        public IEnumerable<LikeModel> GetLikes(Guid activityId)
        {
            return Get(activityId).Likes;
        }

        public void Notify(Guid entityId, NotificationTypeEnum notificationType)
        {
            var notifierData = GetNotifierData(entityId, notificationType);
            if (notifierData != null)
            {
                _notificationService.ProcessNotification(notifierData);
            }
        }

        private NotifierData GetNotifierData(Guid entityId, NotificationTypeEnum notificationType)
        {
            News.Entities.News news;
            var currentUser = _intranetUserService.GetCurrentUser();
            var data = new NotifierData
            {
                NotificationType = notificationType
            };

            switch (notificationType)
            {
                case NotificationTypeEnum.LikeAdded:
                    {
                        news = Get(entityId);
                        data.ReceiverIds = news.CreatorId.ToEnumerableOfOne();
                        data.Value = new LikesNotifierDataModel()
                        {
                            Url = GetDetailsPage().Url.UrlWithQueryString("id", news.Id),
                            Title = news.Title,
                            ActivityType = IntranetActivityTypeEnum.News,
                            NotifierId = currentUser.Id,
                            NotifierName = currentUser.DisplayedName
                        };
                    }
                    break;
                case NotificationTypeEnum.CommentAdded:
                case NotificationTypeEnum.CommentEdited:
                    {
                        var comment = _commentsService.Get(entityId);
                        news = Get(comment.ActivityId);
                        data.ReceiverIds = news.CreatorId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel()
                        {
                            ActivityType = IntranetActivityTypeEnum.News,
                            NotifierId = comment.UserId,
                            NotifierName = _intranetUserService.Get(comment.UserId).DisplayedName,
                            Title = news.Title,
                            Url = GetUrlWithComment(news.Id, comment.Id)
                        };
                    }
                    break;
                case NotificationTypeEnum.CommentReplyed:
                    {
                        var comment = _commentsService.Get(entityId);
                        news = Get(comment.ActivityId);
                        data.ReceiverIds = comment.UserId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel
                        {
                            ActivityType = IntranetActivityTypeEnum.Ideas,
                            NotifierId = currentUser.Id,
                            NotifierName = currentUser.DisplayedName,
                            Title = news.Title,
                            Url = GetUrlWithComment(news.Id, comment.Id),
                            CommentId = comment.Id
                        };
                    }
                    break;
                default:
                    return null;
            }
            return data;
        }

        private string GetUrlWithComment(Guid newsId, Guid commentId)
        {
            return $"{GetDetailsPage().Url.UrlWithQueryString("id", newsId)}#{_commentsService.GetCommentViewId(commentId)}";
        }
    }
}