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

namespace Compent.uIntra.Controllers
{
    public class NewsController : NewsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/News/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/News/ItemView.cshtml";
        protected override string CreateViewPath => "~/Views/News/CreateView.cshtml";
        protected override string EditViewPath => "~/Views/News/EditView.cshtml";        

        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IGroupService _groupService;

        public NewsController(
            IIntranetUserService<IIntranetUser> intranetUserService,
            INewsService<News> newsService,
            IMediaHelper mediaHelper,
            IIntranetUserContentHelper intranetUserContentHelper,
            IActivityTypeProvider activityTypeProvider, 
            IDocumentIndexer documentIndexer, IGroupService groupService)
            : base(intranetUserService, newsService, mediaHelper, intranetUserContentHelper, activityTypeProvider)
        {
            _intranetUserService = intranetUserService;
            _documentIndexer = documentIndexer;
            _groupService = groupService;
        }

        public ActionResult CentralFeedItem(News item, ActivityLinks links)
        {
            var activity = item;

            var extendedModel = GetItemViewModel(activity, links).Map<NewsExtendedItemViewModel>();
            extendedModel.LikesInfo = activity;
            return PartialView(ItemViewPath, extendedModel);
        }

        public ActionResult PreviewItem(News item)
        {
            FillLinks();
            var activity = item;
            NewsPreviewViewModel viewModel = GetPreviewViewModel(activity);
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
            var groupId = _groupService.GetGroupIdFromQuery(Request.QueryString.ToString());
            if (groupId.HasValue)
            {
                _groupService.AddGroupActivityRelation(groupId.Value, activityId);
            }
        }
    }

}