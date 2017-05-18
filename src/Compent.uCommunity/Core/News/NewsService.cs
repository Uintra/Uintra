using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uCommunity.Core.News.Entities;
using uCommunity.CentralFeed;
using uCommunity.CentralFeed.Entities;
using uCommunity.Comments;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Entities;
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
using uCommunity.Tagging;
using uCommunity.Users.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uCommunity.Core.News
{
    public class NewsService : NewsServiceBase<NewsEntity>,
        INewsService<NewsEntity>,
        ICentralFeedItemService,
        ICommentableService,
        ILikeableService,
        INotifyableService
    {
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISubscribeService _subscribeService;
        private readonly IPermissionsService _permissionsService;
        private readonly INotificationsService _notificationService;
        private readonly ITagsService _tagsService;

        public NewsService(IIntranetActivityRepository intranetActivityRepository,
            ICacheService cacheService,
            IIntranetUserService<IntranetUser> intranetUserService,
            ICommentsService commentsService,
            ILikesService likesService,
            ISubscribeService subscribeService,
            UmbracoHelper umbracoHelper,
            IPermissionsService permissionsService,
            INotificationsService notificationService,
            ITagsService tagsService)
            : base(intranetActivityRepository, cacheService, intranetUserService)
        {
            _intranetUserService = intranetUserService;
            _commentsService = commentsService;
            _likesService = likesService;
            _umbracoHelper = umbracoHelper;
            _permissionsService = permissionsService;
            _subscribeService = subscribeService;
            _notificationService = notificationService;
            _tagsService = tagsService;
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


        public override IntranetActivityTypeEnum ActivityType => IntranetActivityTypeEnum.News;

        public override bool CanEdit(IIntranetActivity cached)
        {
            var currentUser = _intranetUserService.GetCurrentUser();
            var creatorId = Get(cached.Id).CreatorId;
            if (creatorId == currentUser.Id)
            {
                return true;
            }
            if (currentUser.Role != IntranetRolesEnum.WebMaster)
            {
                return false;
            }

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

        protected override void MapBeforeCache(IList<IIntranetActivity> cached)
        {
            foreach (var activity in cached)
            {
                var entity = activity as NewsEntity;
                _subscribeService.FillSubscribers(entity);
                _intranetUserService.FillCreator(entity);
                _commentsService.FillComments(entity);
                _likesService.FillLikes(entity);
                _tagsService.FillTags(entity);
            }
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

        public void Notify(Guid entityId, NotificationTypeEnum notificationType)
        {
            var notifierData = GetNotifierData(entityId, notificationType);
            if (notifierData != null)
            {
                _notificationService.ProcessNotification(notifierData);
            }
        }

        public override IPublishedContent GetOverviewPage(IPublishedContent currentPage)
        {
            return GetOverviewPage();
        }

        public override IPublishedContent GetDetailsPage(IPublishedContent currentPage)
        {
            return GetDetailsPage();
        }

        public override IPublishedContent GetCreatePage(IPublishedContent currentPage)
        {
            return GetCreatePage();
        }

        public override IPublishedContent GetEditPage(IPublishedContent currentPage)
        {
            return GetEditPage();
        }

        private NotifierData GetNotifierData(Guid entityId, NotificationTypeEnum notificationType)
        {
            NewsEntity newsEntity;
            var currentUser = _intranetUserService.GetCurrentUser();
            var data = new NotifierData
            {
                NotificationType = notificationType
            };

            switch (notificationType)
            {
                case NotificationTypeEnum.ActivityLikeAdded:
                    {
                        newsEntity = Get(entityId);
                        data.ReceiverIds = newsEntity.CreatorId.ToEnumerableOfOne();
                        data.Value = new LikesNotifierDataModel()
                        {
                            Url = GetDetailsPage().Url.UrlWithQueryString("id", newsEntity.Id),
                            Title = newsEntity.Title,
                            ActivityType = IntranetActivityTypeEnum.News,
                            NotifierId = currentUser.Id,
                        };
                    }
                    break;
                case NotificationTypeEnum.CommentLikeAdded:
                    {
                        var comment = _commentsService.Get(entityId);
                        newsEntity = Get(comment.ActivityId);
                        data.ReceiverIds = newsEntity.CreatorId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel
                        {
                            CommentId = entityId,
                            ActivityType = IntranetActivityTypeEnum.Events,
                            NotifierId = currentUser.Id,
                            Title = newsEntity.Title,
                            Url = GetUrlWithComment(newsEntity.Id, comment.Id)
                        };
                    }
                    break;

                case NotificationTypeEnum.CommentAdded:
                case NotificationTypeEnum.CommentEdited:
                    {
                        var comment = _commentsService.Get(entityId);
                        newsEntity = Get(comment.ActivityId);
                        data.ReceiverIds = newsEntity.CreatorId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel()
                        {
                            ActivityType = IntranetActivityTypeEnum.News,
                            NotifierId = comment.UserId,
                            Title = newsEntity.Title,
                            Url = GetUrlWithComment(newsEntity.Id, comment.Id)
                        };
                    }
                    break;
                case NotificationTypeEnum.CommentReplyed:
                    {
                        var comment = _commentsService.Get(entityId);
                        newsEntity = Get(comment.ActivityId);
                        data.ReceiverIds = comment.UserId.ToEnumerableOfOne();
                        data.Value = new CommentNotifierDataModel
                        {
                            ActivityType = IntranetActivityTypeEnum.Ideas,
                            NotifierId = currentUser.Id,
                            Title = newsEntity.Title,
                            Url = GetUrlWithComment(newsEntity.Id, comment.Id),
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