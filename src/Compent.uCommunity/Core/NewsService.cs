using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.CentralFeed;
using uCommunity.CentralFeed.Entities;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Caching;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.News;
using Umbraco.Core.Models;

namespace Compent.uCommunity.Core
{
    public class NewsService : IntranetActivityItemServiceBase<NewsBase, News.News>, INewsService<NewsBase, News.News>, ICentralFeedItemService
    {
        private readonly IIntranetUserService _intranetUserService;

        public NewsService(IIntranetActivityService intranetActivityService, IMemoryCacheService memoryCacheService, IIntranetUserService intranetUserService)
            : base(intranetActivityService, memoryCacheService)
        {
            _intranetUserService = intranetUserService;
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
            return new PublishedContentCustom(1073, "/news");
        }

        public override IPublishedContent GetDetailsPage()
        {
            return new PublishedContentCustom(1075, "/news/details");
        }

        public override IPublishedContent GetCreatePage()
        {
            return new PublishedContentCustom(1074, "/news/create");
        }

        public override IPublishedContent GetEditPage()
        {
            return new PublishedContentCustom(1076, "/news/edit");
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

        public bool IsActual(News.News activity)
        {
            return base.IsActual(activity) && activity.PublishDate.Date <= DateTime.Now.Date;
        }

        public ICentralFeedItem GetItem(Guid activityId)
        {
            var news = Get(activityId);
            return (ICentralFeedItem) news;
        }

        public IEnumerable<ICentralFeedItem> GetItems()
        {
            var items = GetManyActual().OrderByDescending(i => i.PublishDate);

            return  items;
        }

        protected override News.News FillPropertiesOnGet(IntranetActivityEntity entity)
        {
            var activity = base.FillPropertiesOnGet(entity);
            _intranetUserService.FillCreator(activity);
            return activity;
        }
    }
}