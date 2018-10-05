using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Compent.Uintra.Core.Activity.Models;
using Compent.Uintra.Core.Feed;
using Compent.Uintra.Core.News.Entities;
using Compent.Uintra.Core.News.Models;
using Uintra.Core.Extensions;
using Uintra.Core.Links;
using Uintra.Core.Media;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Groups;
using Uintra.News;
using Uintra.News.Web;
using Uintra.Search;
using Compent.Uintra.Core.UserTags;
using Uintra.Core;
using Uintra.Core.Activity;
using Uintra.Groups.Extentions;
using Uintra.Tagging.UserTags;
using Uintra.Users;

namespace Compent.Uintra.Controllers
{
    public class NewsController : NewsControllerBase
    {
        protected override string DetailsViewPath => "~/Views/News/DetailsView.cshtml";
        protected override string ItemViewPath => "~/Views/News/ItemView.cshtml";
        protected override string CreateViewPath => "~/Views/News/CreateView.cshtml";
        protected override string EditViewPath => "~/Views/News/EditView.cshtml";

        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly INewsService<News> _newsService;
        private readonly IDocumentIndexer _documentIndexer;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IActivityLinkService _activityLinkService;
        private readonly IMentionService _mentionService;

        public NewsController(
            IIntranetUserService<IIntranetUser> intranetUserService,
            INewsService<News> newsService,
            IMediaHelper mediaHelper,
            IIntranetUserContentProvider intranetUserContentProvider,
            IActivityTypeProvider activityTypeProvider,
            IDocumentIndexer documentIndexer,
            IGroupActivityService groupActivityService,
            UserTagService userTagService,
            IActivityTagsHelper activityTagsHelper,
            IActivityLinkService activityLinkService,
            IContextTypeProvider contextTypeProvider,
            IMentionService mentionService)
            : base(intranetUserService, newsService, mediaHelper, activityTypeProvider, activityLinkService, contextTypeProvider)
        {
            _intranetUserService = intranetUserService;
            _newsService = newsService;
            _documentIndexer = documentIndexer;
            _groupActivityService = groupActivityService;
            _activityTagsHelper = activityTagsHelper;
            _activityLinkService = activityLinkService;
            _mentionService = mentionService;
        }

        public ActionResult FeedItem(News item, ActivityFeedOptionsWithGroups options)
        {
            var extendedModel = GetItemViewModel(item, options);
            AddEntityIdentityForContext(item.Id);
            return PartialView(ItemViewPath, extendedModel);
        }



        private NewsExtendedItemViewModel GetItemViewModel(News item, ActivityFeedOptionsWithGroups options)
        {
            var model = GetItemViewModel(item, options.Links);
            var extendedModel = model.Map<NewsExtendedItemViewModel>();

            extendedModel.HeaderInfo = model.HeaderInfo.Map<ExtendedItemHeaderViewModel>();
            extendedModel.HeaderInfo.GroupInfo = options.GroupInfo;

            extendedModel.LikesInfo = item;
            extendedModel.LikesInfo.IsReadOnly = options.IsReadOnly;
            extendedModel.IsReadOnly = options.IsReadOnly;
            return extendedModel;
        }

        public ActionResult PreviewItem(News item, ActivityLinks links)
        {
            NewsPreviewViewModel viewModel = GetPreviewViewModel(item, links);
            AddEntityIdentityForContext(item.Id);
            return PartialView(PreviewItemViewPath, viewModel);
        }

        [HttpPost]
        public ActionResult EditExtended(NewsExtendedEditModel editModel)
        {
            return Edit(editModel);
        }

        [HttpPost]
        public ActionResult CreateExtended(NewsExtendedCreateModel createModel)
        {
            return Create(createModel);
        }

        protected override NewsEditModel GetEditViewModel(NewsBase news, ActivityLinks links)
        {
            var extendedModel = base.GetEditViewModel(news, links).Map<NewsExtendedEditModel>();
            //extendedModel.TagIdsData = _userTagService.GetRelatedTags(extendedModel.Id).JoinToString();
            return extendedModel;
        }

        protected override NewsCreateModel GetCreateModel(IActivityCreateLinks links)
        {
            var extendedModel = base.GetCreateModel(links).Map<NewsExtendedCreateModel>();
            return extendedModel;
        }

        protected override void OnNewsEdited(NewsBase news, NewsEditModel model)
        {
            if (model is NewsExtendedEditModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(news.Id, extendedModel.TagIdsData);
            }

            ResolveMentions(model.Description, news);
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
            var news = _newsService.Get(activityId);
            var groupId = Request.QueryString.GetGroupId();
            if (groupId.HasValue)
            {
                _groupActivityService.AddRelation(groupId.Value, activityId);
                news.GroupId = groupId;
            }
            if (model is NewsExtendedCreateModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(activityId, extendedModel.TagIdsData);
            }

            ResolveMentions(model.Description, news);
        }     

        private void ResolveMentions(string text, NewsBase news)
        {
            var mentionIds = _mentionService.GetMentions(text).ToList();

            if (mentionIds.Any())
            {
                var links = _activityLinkService.GetLinks(news.Id);
                _mentionService.ProcessMention(new MentionModel()
                {
                    MentionedSourceId = news.Id,
                    CreatorId = _intranetUserService.GetCurrentUserId(),
                    MentionedUserIds = mentionIds,
                    Title = news.Title,
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.News
                });

            }
        }
    }
}