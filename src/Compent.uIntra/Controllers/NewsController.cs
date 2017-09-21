using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Compent.uIntra.Core.News.Entities;
using Compent.uIntra.Core.News.Models;
using uIntra.Core.Extentions;
using uIntra.Core.Links;
using uIntra.Core.Media;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.News;
using uIntra.News.Web;
using uIntra.Search;
using System.Linq;
using Compent.uIntra.Core.Extentions;
using uIntra.CentralFeed;
using uIntra.Groups.Extentions;

namespace Compent.uIntra.Controllers
{
    public class NewsController : NewsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/News/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/News/ItemView.cshtml";
        protected override string CreateViewPath => "~/Views/News/CreateView.cshtml";
        protected override string EditViewPath => "~/Views/News/EditView.cshtml";

        private readonly INewsService<News> _newsService;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IGroupActivityService _groupActivityService;


        public NewsController(
            IIntranetUserService<IIntranetUser> intranetUserService,
            INewsService<News> newsService,
            IMediaHelper mediaHelper,
            IIntranetUserContentHelper intranetUserContentHelper,
            IActivityTypeProvider activityTypeProvider, 
            IDocumentIndexer documentIndexer,
            IGroupActivityService groupActivityService)
            : base(intranetUserService, newsService, mediaHelper, intranetUserContentHelper, activityTypeProvider)
        {
            _newsService = newsService;
            _documentIndexer = documentIndexer;
            _groupActivityService = groupActivityService;
        }

        public ActionResult CentralFeedItem(News item, FeedOptionsModel options)
        {
            var activity = item;

            var extendedModel = GetItemViewModel(activity, options.Links).Map<NewsExtendedItemViewModel>();
            extendedModel.LikesInfo = activity;
            extendedModel.LikesInfo.IsReadOnly = options.IsReadOnly;
            extendedModel.IsReadOnly = options.IsReadOnly;
            return PartialView(ItemViewPath, extendedModel);
        }

        public ActionResult PreviewItem(News item, ActivityLinks links)
        {
            NewsPreviewViewModel viewModel = GetPreviewViewModel(item, links);
            return PartialView(PreviewItemViewPath, viewModel);
        }

        protected override NewsViewModel GetViewModel(NewsBase news)
        {
            var extendedNews = (News)news;
            var extendedModel = base.GetViewModel(news).Map<NewsExtendedViewModel>();
            extendedModel = Mapper.Map(extendedNews, extendedModel);
            
            return extendedModel;
        }

        protected override void DeleteMedia(IEnumerable<int> mediaIds)
        {
            base.DeleteMedia(mediaIds);
            _documentIndexer.DeleteFromIndex(mediaIds);
        }

        protected override void OnNewsCreated(Guid activityId, NewsCreateModel model)
        {
            var groupId = Request.QueryString.GetGroupId();
            if (groupId.HasValue)
            {
                _groupActivityService.AddRelation(groupId.Value, activityId);
                var news = _newsService.Get(activityId);
                news.GroupId = groupId;
            }
        }
    }
}