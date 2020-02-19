using AutoMapper;
using Compent.Shared.Extensions.Bcl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using UBaseline.Core.Controllers;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Helpers;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.News.Models;
using Uintra20.Features.Notification;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Social;
using Uintra20.Features.Tagging.UserTags;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.News.Controllers
{
    public class NewsApiController : UBaselineApiController,IFeedHub
    {
        private const PermissionResourceTypeEnum ActivityType = PermissionResourceTypeEnum.News;

        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly INewsService<Entities.News> _newsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IActivityLinkService _activityLinkService;
        private readonly IMentionService _mentionService;
        private readonly IPermissionsService _permissionsService;
        private readonly IMemberServiceHelper _memberHelper;

        public NewsApiController(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            INewsService<Entities.News> newsService,
            IMediaHelper mediaHelper,
            //IDocumentIndexer documentIndexer,
            IGroupActivityService groupActivityService,
            IActivityTagsHelper activityTagsHelper,
            IActivityLinkService activityLinkService,
            IMentionService mentionService,
            IPermissionsService permissionsService,
            IMemberServiceHelper memberHelper)
        {
            _intranetMemberService = intranetMemberService;
            _newsService = newsService;
            _mediaHelper = mediaHelper;
            _groupActivityService = groupActivityService;
            _activityTagsHelper = activityTagsHelper;
            _activityLinkService = activityLinkService;
            _mentionService = mentionService;
            _permissionsService = permissionsService;
            _memberHelper = memberHelper;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(NewsCreateModel createModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_permissionsService.Check(PermissionSettingIdentity.Of(PermissionActionEnum.Create,
                PermissionResourceTypeEnum.News)))
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            var newsBaseCreateModel = await MapToNewsAsync(createModel);
            var activityId = await _newsService.CreateAsync(newsBaseCreateModel);

            await OnNewsCreatedAsync(activityId, createModel);
            var newsViewModel = await GetViewModelAsync(_newsService.Get(activityId));

            ReloadFeed();
            return Ok(newsViewModel.Links.Details);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Edit(NewsEditModel editModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_permissionsService.Check(PermissionSettingIdentity.Of(PermissionActionEnum.Edit,
                PermissionResourceTypeEnum.News)))
            {
                return Ok((await _activityLinkService.GetLinksAsync(editModel.Id)).Details);
            }

            var cachedActivityMedias = _newsService.Get(editModel.Id).MediaIds;

            var activity = MapToNews(editModel);
            await _newsService.SaveAsync(activity);

            DeleteMedia(cachedActivityMedias.Except(activity.MediaIds));

            await OnNewsEditedAsync(activity, editModel);
            var newsViewModel = await GetViewModelAsync(_newsService.Get(editModel.Id));

            ReloadFeed();
            return Ok(newsViewModel.Links.Details);
        }

        public void ReloadFeed()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<UintraHub>();
            hubContext.Clients.All.reloadFeed();
        }

        private async Task<NewsBase> MapToNewsAsync(NewsCreateModel createModel)
        {
            var news = createModel.Map<NewsBase>();

            news.MediaIds = news.MediaIds.Concat(_mediaHelper.CreateMedia(createModel, MediaFolderTypeEnum.NewsContent));
            news.PublishDate = createModel.PublishDate.ToUniversalTime().WithCorrectedDaylightSavingTime(createModel.PublishDate);
            news.UnpublishDate = createModel.UnpublishDate?.ToUniversalTime().WithCorrectedDaylightSavingTime(createModel.UnpublishDate.Value);
            news.EndPinDate = createModel.EndPinDate?.ToUniversalTime().WithCorrectedDaylightSavingTime(createModel.EndPinDate.Value);
            news.CreatorId = await _intranetMemberService.GetCurrentMemberIdAsync();

            if (await IsPinAllowedAsync())
                return news;

            news.IsPinned = createModel.IsPinned;

            return news;
        }
        private NewsBase MapToNews(NewsEditModel editModel)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            var activity = _newsService.Get(editModel.Id);
            activity = Mapper.Map(editModel, activity);
            activity.MediaIds = activity.MediaIds.Concat(_mediaHelper.CreateMedia(editModel, MediaFolderTypeEnum.NewsContent));
            activity.PublishDate = editModel.PublishDate.ToUniversalTime().WithCorrectedDaylightSavingTime(editModel.PublishDate);
            activity.UnpublishDate = editModel.UnpublishDate?.ToUniversalTime().WithCorrectedDaylightSavingTime(editModel.UnpublishDate.Value);
            activity.EndPinDate = editModel.EndPinDate?.ToUniversalTime().WithCorrectedDaylightSavingTime(editModel.EndPinDate.Value);

            activity.IsPinned = _permissionsService.Check(
                                    currentMember, 
                                    new PermissionSettingIdentity(PermissionActionEnum.CanPin, IntranetActivityTypeEnum.News)) 
                                && activity.IsPinned;

            return activity;
        }
        private async Task OnNewsEditedAsync(NewsBase news, NewsEditModel model)
        {

            await _activityTagsHelper.ReplaceTagsAsync(news.Id, model.TagIdsData);

            await ResolveMentionsAsync(model.Description, news);
        }

        private void DeleteMedia(IEnumerable<int> mediaIds)
        {
            _mediaHelper.DeleteMedia(mediaIds);
        }
        private Task<bool> IsPinAllowedAsync()
        {
            return _permissionsService.CheckAsync(ActivityType, PermissionActionEnum.CanPin);
        }
        private async Task<NewsViewModel> GetViewModelAsync(NewsBase news)
        {
            var model = news.Map<NewsViewModel>();

            model.HeaderInfo = news.Map<IntranetActivityDetailsHeaderViewModel>();
            model.HeaderInfo.Dates = news.PublishDate.ToDateTimeFormat().ToEnumerable();
            model.HeaderInfo.Owner = _memberHelper.ToViewModel(await _intranetMemberService.GetAsync(news));
            model.Links = await _activityLinkService.GetLinksAsync(model.Id);
            model.HeaderInfo.Links = await _activityLinkService.GetLinksAsync(model.Id);
            model.CanEdit = _newsService.CanEdit(news);

            return model;
        }
        private async Task OnNewsCreatedAsync(Guid activityId, NewsCreateModel model)
        {
            var news = _newsService.Get(activityId);
            var groupId = HttpContext.Current.Request.QueryString.GetGroupIdOrNone();

            if (groupId.HasValue)
            {
                await _groupActivityService.AddRelationAsync(groupId.Value, activityId);
                news.GroupId = groupId.Value;
            }

            await _activityTagsHelper.ReplaceTagsAsync(activityId, model.TagIdsData);

            await ResolveMentionsAsync(model.Description, news);
        }
        private async Task ResolveMentionsAsync(string text, NewsBase news)
        {
            var mentionIds = _mentionService.GetMentions(text).ToArray();

            if (!mentionIds.Any())
                return;

            var links = await _activityLinkService.GetLinksAsync(news.Id);
            _mentionService.ProcessMention(
                new MentionModel
                {
                    MentionedSourceId = news.Id,
                    CreatorId = await _intranetMemberService.GetCurrentMemberIdAsync(),
                    MentionedUserIds = mentionIds,
                    Title = news.Title?.StripHtml(),
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.News
                });

        }
    }
}