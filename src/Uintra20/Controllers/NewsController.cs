using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using UBaseline.Shared.NewsPreview;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Feed;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Media;
using Uintra20.Features.News;
using Uintra20.Features.News.Entities;
using Uintra20.Features.News.Web;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Tagging.UserTags;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Controllers
{
    public class NewsController : NewsControllerBase
    {
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly INewsService<News> _newsService;
        //private readonly IDocumentIndexer _documentIndexer;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IActivityLinkService _activityLinkService;
        private readonly IMentionService _mentionService;

        public NewsController(
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            INewsService<News> newsService,
            IMediaHelper mediaHelper,
            IActivityTypeProvider activityTypeProvider,
            //IDocumentIndexer documentIndexer,
            IGroupActivityService groupActivityService,
            IActivityTagsHelper activityTagsHelper,
            IActivityLinkService activityLinkService,
            IMentionService mentionService,
            IPermissionsService permissionsService)
            : base(intranetMemberService, newsService, mediaHelper, activityTypeProvider, activityLinkService, permissionsService)
        {
            _intranetMemberService = intranetMemberService;
            _newsService = newsService;
            //_documentIndexer = documentIndexer;
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

        //protected override NewsCreateModel GetCreateModel(IActivityCreateLinks links)
        //{
        //    var extendedModel = base.GetCreateModel(links).Map<NewsExtendedCreateModel>();
        //    return extendedModel;
        //}

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
            var groupId = Request.QueryString.GetGroupIdOrNone();

            groupId.IfSome(id =>
            {
                _groupActivityService.AddRelation(id, activityId);
                news.GroupId = id;
            });

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
                    CreatorId = _intranetMemberService.GetCurrentMemberId(),
                    MentionedUserIds = mentionIds,
                    Title = news.Title.StripHtml(),
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.News
                });

            }
        }
    }
}