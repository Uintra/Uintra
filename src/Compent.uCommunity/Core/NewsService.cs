using System.Collections.Generic;
using uCommunity.Core.Activity;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Caching;
using uCommunity.Core.Media;
using uCommunity.Core.User;
using uCommunity.News;
using Umbraco.Core.Models;

namespace Compent.uCommunity.Core
{
    public class NewsService : IntranetActivityItemServiceBase<NewsBase, NewsModelBase>, INewsService<NewsBase, NewsModelBase>
    {
        private readonly IIntranetUserService<IntranetUserBase> _intranetUserService;

        public NewsService(IIntranetActivityService intranetActivityService, IMemoryCacheService memoryCacheService, IIntranetUserService<IntranetUserBase> intranetUserService)
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

        protected override NewsModelBase FillPropertiesOnGet(IntranetActivityEntity entity)
        {
            var activity = base.FillPropertiesOnGet(entity);
            _intranetUserService.FillCreator(activity);
            return activity;
        }
    }
}