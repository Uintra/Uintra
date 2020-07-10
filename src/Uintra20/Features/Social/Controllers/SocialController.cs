﻿using AutoMapper;
using Compent.Shared.Extensions.Bcl;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Attributes;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Core.Controls.LightboxGallery;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media.Enums;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.Navigation.Services;
using Uintra20.Features.Notification;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Social.Models;
using Uintra20.Features.Tagging.UserTags;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Social.Controllers
{
    [ValidateModel]
    public class SocialController : UBaselineApiController, IFeedHub
    {
        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IMyLinksService _myLinksService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IMentionService _mentionService;
        private readonly IActivityLinkService _activityLinkService;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IFeedLinkService _feedLinkService;
        private readonly IPermissionsService _permissionsService;

        public SocialController(
            ISocialService<Entities.Social> socialService,
            IMediaHelper mediaHelper,
            IIntranetMemberService<IntranetMember> memberService,
            IMyLinksService myLinksService,
            IGroupActivityService groupActivityService,
            IActivityTagsHelper activityTagsHelper,
            IMentionService mentionService,
            IActivityLinkService activityLinkService,
            ILightboxHelper lightboxHelper,
            IFeedLinkService feedLinkService,
            IPermissionsService permissionsService)
        {
            _socialService = socialService;
            _mediaHelper = mediaHelper;
            _memberService = memberService;
            _myLinksService = myLinksService;
            _groupActivityService = groupActivityService;
            _activityTagsHelper = activityTagsHelper;
            _mentionService = mentionService;
            _activityLinkService = activityLinkService;
            _lightboxHelper = lightboxHelper;
            _feedLinkService = feedLinkService;
            _permissionsService = permissionsService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(SocialCreateModel social)
        {
            //if (!_permissionsService.Check(new PermissionSettingIdentity(PermissionActionEnum.Create, PermissionResourceTypeEnum.Social)))
            if (!await _socialService.CanCreateAsync(social.GroupId))
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            var mappedSocial = MapToSocial(social);

            var socialId = await _socialService.CreateAsync(mappedSocial);

            mappedSocial.Id = socialId;

            await OnBulletinCreatedAsync(mappedSocial, social);

            ReloadFeed();

            var result = await GetSocialViewModelAsync(socialId);

            return Ok(result.Links.Details);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Update(SocialEditModel socialEdit)
        {
            //if (!_permissionsService.Check(new PermissionSettingIdentity(PermissionActionEnum.Edit, PermissionResourceTypeEnum.Social)))
            if (!await _socialService.CanEditAsync(socialEdit.Id))
            {
                return Ok((await _activityLinkService.GetLinksAsync(socialEdit.Id)).Details);
            }

            var social = MapToSocial(socialEdit);

            await _socialService.SaveAsync(social);

            await OnSocialEditedAsync(social, socialEdit);

            var model = await GetSocialViewModelAsync(social.Id);

            ReloadFeed();

            return Ok(model.Links.Details);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            if (!await _socialService.CanDeleteAsync(id))
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            await _socialService.DeleteAsync(id);

            await OnBulletinDeletedAsync(id);

            ReloadFeed();
            return Ok();
        }

        public void ReloadFeed() =>
            GlobalHost.ConnectionManager.GetHubContext<UintraHub>().Clients.All.reloadFeed();

        private SocialBase MapToSocial(SocialCreateModel model)
        {
            var bulletin = model.Map<SocialBase>();
            bulletin.PublishDate = DateTime.UtcNow;
            bulletin.CreatorId = bulletin.OwnerId = _memberService.GetCurrentMemberId();

            if (model.NewMedia.HasValue())
            {
                bulletin.MediaIds = _mediaHelper.CreateMedia(model, MediaFolderTypeEnum.SocialsContent);
            }

            return bulletin;
        }

        protected async Task<SocialExtendedViewModel> GetSocialViewModelAsync(Guid id)
        {
            var social = _socialService.Get(id);

            var viewModel = social.Map<SocialViewModel>();

            viewModel.Media = MediaHelper.GetMediaUrls(social.MediaIds);

            viewModel.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(social.MediaIds, PresetStrategies.ForActivityDetails);
            viewModel.CanEdit = _socialService.CanEdit(social);
            viewModel.Links = await _feedLinkService.GetLinksAsync(id);
            viewModel.IsReadOnly = false;
            viewModel.HeaderInfo = social.Map<IntranetActivityDetailsHeaderViewModel>();
            viewModel.HeaderInfo.Dates = social.PublishDate.ToDateTimeFormat().ToEnumerable();
            viewModel.HeaderInfo.Owner = _memberService.Get(social).ToViewModel();
            viewModel.HeaderInfo.Links = await _feedLinkService.GetLinksAsync(id);

            var extendedModel = viewModel.Map<SocialExtendedViewModel>();

            return extendedModel;
        }

        private SocialBase MapToSocial(SocialEditModel socialEditModel)
        {
            var social = _socialService.Get(socialEditModel.Id);

            social = socialEditModel.Map(social);

            social.MediaIds = social.MediaIds.Concat(_mediaHelper.CreateMedia(socialEditModel, MediaFolderTypeEnum.SocialsContent));

            return social;
        }

        private void OnBulletinEdited(SocialBase social, SocialEditModel model)
        {

            _activityTagsHelper.ReplaceTags(social.Id, model.TagIdsData);

            ResolveMentions(model.Description, social);
        }

        private async Task OnSocialEditedAsync(SocialBase social, SocialEditModel model)
        {
            await _activityTagsHelper.ReplaceTagsAsync(social.Id, model.TagIdsData);

            await ResolveMentionsAsync(model.Description, social);
        }

        private void OnBulletinDeleted(Guid id)
        {
            _myLinksService.DeleteByActivityId(id);
        }

        private async Task OnBulletinDeletedAsync(Guid id)
        {
            await _myLinksService.DeleteByActivityIdAsync(id);

        }

        private void OnBulletinCreated(SocialBase social, SocialCreateModel model)
        {
            var groupId = HttpContext.Current.Request.QueryString.GetGroupIdOrNone();

            if (groupId.HasValue)
                _groupActivityService.AddRelation(groupId.Value, social.Id);

            var extendedBulletin = _socialService.Get(social.Id);
            extendedBulletin.GroupId = groupId;


            _activityTagsHelper.ReplaceTags(social.Id, model.TagIdsData);

            if (model.Description.HasValue())
            {
                ResolveMentions(model.Description, social);
            }
        }

        private async Task OnBulletinCreatedAsync(SocialBase social, SocialCreateModel model)
        {
            if (model.GroupId.HasValue)
                await _groupActivityService.AddRelationAsync(model.GroupId.Value, social.Id);

            var extendedBulletin = _socialService.Get(social.Id);
            extendedBulletin.GroupId = model.GroupId;

            await _activityTagsHelper.ReplaceTagsAsync(social.Id, model.TagIdsData);

            if (model.Description.HasValue())
            {
                await ResolveMentionsAsync(model.Description, social);
            }
        }

        private void ResolveMentions(string text, SocialBase social)
        {
            var mentionIds = new Guid[] { };//_mentionService.GetMentions(text).ToList();//TODO: uncomment when mention service is ready

            if (mentionIds.Any())
            {
                var links = _activityLinkService.GetLinks(social.Id);
                const int maxTitleLength = 100;
                _mentionService.ProcessMention(new MentionModel()
                {
                    MentionedSourceId = social.Id,
                    CreatorId = _memberService.GetCurrentMemberId(),
                    MentionedUserIds = mentionIds,
                    Title = social.Description.StripHtml().TrimByWordEnd(maxTitleLength),
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.Social
                });

            }
        }

        private async Task ResolveMentionsAsync(string text, SocialBase social)
        {
            var mentionIds = _mentionService.GetMentions(text).ToList();

            if (mentionIds.Any())
            {
                var links = await _activityLinkService.GetLinksAsync(social.Id);
                const int maxTitleLength = 100;
                _mentionService.ProcessMention(new MentionModel()
                {
                    MentionedSourceId = social.Id,
                    CreatorId = await _memberService.GetCurrentMemberIdAsync(),
                    MentionedUserIds = mentionIds,
                    Title = social.Description.StripMentionHtml().TrimByWordEnd(maxTitleLength),
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.Social
                });
            }
        }
    }
}