using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.CentralFeed;
using uCommunity.CentralFeed.Entities;
using uCommunity.Comments;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Caching;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.Likes;
using uCommunity.News;
using uCommunity.Subscribe;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Compent.uCommunity.Core
{
    public class NewsService : IntranetActivityItemServiceBase<NewsBase, News.Entities.News>, INewsService<NewsBase, News.Entities.News>, ICentralFeedItemService, ICommentableService, ILikeableService, ISubscribableService
    {
        private readonly IIntranetUserService _intranetUserService;
        private readonly ICommentsService _commentsService;
        private readonly ILikesService _likesService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ISubscribeService _subscribeService;

        public NewsService(IIntranetActivityService intranetActivityService,
            IMemoryCacheService memoryCacheService,
            IIntranetUserService intranetUserService,
            ICommentsService commentsService,
            ILikesService likesService, 
            ISubscribeService subscribeService,
            UmbracoHelper umbracoHelper)
            : base(intranetActivityService, memoryCacheService)
        {
            _intranetUserService = intranetUserService;
            _commentsService = commentsService;
            _likesService = likesService;
            _umbracoHelper = umbracoHelper;
            _subscribeService = subscribeService;
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
            return true;
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

            /*if (parentId.HasValue)
            {
                Notify(parentId.Value, NotificationTypeEnum.CommentReplyed);
            }
            else
            {
                Notify(comment.Id, NotificationTypeEnum.CommentAdded);
            }*/
        }

        public void UpdateComment(Guid id, string text)
        {
            var comment = _commentsService.Update(id, text);
            FillCache(comment.ActivityId);
            //Notify(comment.Id, NotificationTypeEnum.CommentEdited);
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

        public void Add(Guid userId, Guid activityId)
        {
            _likesService.Add(userId, activityId);
            //Notify(activityId, NotificationTypeEnum.LikeAdded);
            FillCache(activityId);
        }

        public void Remove(Guid userId, Guid activityId)
        {
            _likesService.Remove(userId, activityId);
            FillCache(activityId);
        }

        public IEnumerable<LikeModel> GetLikes(Guid activityId)
        {
            return Get(activityId).Likes;
        }

        public global::uCommunity.Subscribe.Subscribe Subscribe(Guid userId, Guid activityId)
        {
            var subscribe = _subscribeService.Subscribe(userId, activityId);
            FillCache(activityId);
            return subscribe;
        }

        public void UnSubscribe(Guid userId, Guid activityId)
        {
            _subscribeService.Unsubscribe(userId, activityId);
            FillCache(activityId);
        }

        public void UpdateNotification(Guid id, bool value)
        {
            var subscribe = _subscribeService.UpdateNotification(id, value);
            FillCache(subscribe.ActivityId);
        }
    }
}