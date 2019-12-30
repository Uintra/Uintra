using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using LanguageExt;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.News;
using Uintra20.Features.News.Entities;
using Uintra20.Features.News.Models;
using Uintra20.Features.News.Web;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Tagging.UserTags;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Controllers
{
    public class NewsController : NewsControllerBase
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly INewsService<News> _newsService;
        //private readonly IDocumentIndexer _documentIndexer;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IActivityLinkService _activityLinkService;
        private readonly IMentionService _mentionService;

        public NewsController(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            INewsService<News> newsService,
            IMediaHelper mediaHelper,
            //IDocumentIndexer documentIndexer,
            IGroupActivityService groupActivityService,
            IActivityTagsHelper activityTagsHelper,
            IActivityLinkService activityLinkService,
            IMentionService mentionService,
            IPermissionsService permissionsService)
            : base(intranetMemberService, newsService, mediaHelper, permissionsService)
        {
            _intranetMemberService = intranetMemberService;
            _newsService = newsService;
            //_documentIndexer = documentIndexer;
            _groupActivityService = groupActivityService;
            _activityTagsHelper = activityTagsHelper;
            _activityLinkService = activityLinkService;
            _mentionService = mentionService;
        }

        [HttpPost]
        public async Task<NewsViewModel> EditExtended(NewsExtendedEditModel editModel)
        {
            return await Edit(editModel);
        }

        [HttpPost]
        public async Task<NewsViewModel> CreateExtended(NewsExtendedCreateModel createModel)
        {
            return await Create(createModel);
        }

        protected override void OnNewsEdited(NewsBase news, NewsEditModel model)
        {
            if (model is NewsExtendedEditModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(news.Id, extendedModel.TagIdsData);
            }

            ResolveMentions(model.Description, news);
        }

        protected virtual async Task OnNewsEditedAsync(NewsBase news, NewsEditModel model)
        {
            if (model is NewsExtendedEditModel extendedModel)
            {
                await _activityTagsHelper.ReplaceTagsAsync(news.Id, extendedModel.TagIdsData);
            }

            await ResolveMentionsAsync(model.Description, news);
        }

        protected override NewsViewModel GetViewModel(NewsBase news)
        {
            var extendedNews = (News)news;
            var extendedModel = base.GetViewModel(news).Map<NewsExtendedViewModel>();
            extendedModel = Mapper.Map(extendedNews, extendedModel);

            return extendedModel;
        }

        //protected override void DeleteMedia(IEnumerable<int> mediaIds)
        //{
        //    base.DeleteMedia(mediaIds);
        //    _documentIndexer.DeleteFromIndex(mediaIds);
        //}

        protected override void OnNewsCreated(Guid activityId, NewsCreateModel model)
        {
            var news = _newsService.Get(activityId);
            var groupId = HttpContext.Current.Request.QueryString.GetGroupIdOrNone();

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

        protected override async Task OnNewsCreatedAsync(Guid activityId, NewsCreateModel model)
        {
            var news = _newsService.Get(activityId);
            var groupId = HttpContext.Current.Request.QueryString.GetGroupIdOrNone();

            if (groupId.HasValue)
            {
                await _groupActivityService.AddRelationAsync(groupId.Value, activityId);
                news.GroupId = groupId.Value;
            }

            if (model is NewsExtendedCreateModel extendedModel)
            {
                await _activityTagsHelper.ReplaceTagsAsync(activityId, extendedModel.TagIdsData);
            }

            await ResolveMentionsAsync(model.Description, news);
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

        private async Task ResolveMentionsAsync(string text, NewsBase news)
        {
            var mentionIds = _mentionService.GetMentions(text).ToList();

            if (mentionIds.Any())
            {
                var links = await _activityLinkService.GetLinksAsync(news.Id);
                _mentionService.ProcessMention(new MentionModel()
                {
                    MentionedSourceId = news.Id,
                    CreatorId = await _intranetMemberService.GetCurrentMemberIdAsync(),
                    MentionedUserIds = mentionIds,
                    Title = news.Title.StripHtml(),
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.News
                });

            }
        }
    }
}