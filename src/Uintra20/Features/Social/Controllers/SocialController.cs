using AutoMapper;
using Compent.Shared.Extensions.Bcl;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Attributes;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Navigation.Services;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Notification;
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
            _feedLinkService = feedLinkService;
            _permissionsService = permissionsService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateExtended(SocialCreateModel model)
        {
            if (!_permissionsService.Check(new PermissionSettingIdentity(PermissionActionEnum.Create,
                PermissionResourceTypeEnum.Social)))
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            var result = new SocialCreationResultModel();

            var bulletin = MapToBulletin(model);
            var createdBulletinId = await _socialService.CreateAsync(bulletin);
            bulletin.Id = createdBulletinId;
            await OnBulletinCreatedAsync(bulletin, model);

            result.Id = createdBulletinId;
            result.IsSuccess = true;

            var links = await _feedLinkService.GetLinksAsync(createdBulletinId);

            ReloadFeed();
            return Ok(links.Details);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Update(SocialEditModel editModel)
        {
            if (!_permissionsService.Check(new PermissionSettingIdentity(PermissionActionEnum.Edit,
                PermissionResourceTypeEnum.Social)))
            {
                return Ok((await _activityLinkService.GetLinksAsync(editModel.Id)).Details);
            }

            var bulletin = MapToBulletin(editModel);

            await _socialService.SaveAsync(bulletin);

            await OnBulletinEditedAsync(bulletin, editModel);

            var links = await _feedLinkService.GetLinksAsync(bulletin.Id);
            ReloadFeed();
            return Ok(links.Details);
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

        public void ReloadFeed()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<UintraHub>();
            hubContext.Clients.All.reloadFeed();
        }

        private SocialBase MapToBulletin(SocialCreateModel model)
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

        private SocialBase MapToBulletin(SocialEditModel socialEditModel)
        {
            var social = _socialService.Get(socialEditModel.Id);

            social = Mapper.Map(socialEditModel, social);

            social.MediaIds = social.MediaIds.Concat(_mediaHelper.CreateMedia(socialEditModel, MediaFolderTypeEnum.SocialsContent));

            return social;
        }

        private async Task OnBulletinEditedAsync(SocialBase social, SocialEditModel model)
        {
            await _activityTagsHelper.ReplaceTagsAsync(social.Id, model.TagIdsData);

            await ResolveMentionsAsync(model.Description, social);
        }

        private async Task OnBulletinDeletedAsync(Guid id)
        {
            await _myLinksService.DeleteByActivityIdAsync(id);

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

        private async Task ResolveMentionsAsync(string text, SocialBase social)
        {
            var mentionIds = new Guid[] { };//_mentionService.GetMentions(text).ToList();//TODO: uncomment when mention service is ready

            if (mentionIds.Any())
            {
                var links = await _activityLinkService.GetLinksAsync(social.Id);
                const int maxTitleLength = 100;
                _mentionService.ProcessMention(new MentionModel
                {
                    MentionedSourceId = social.Id,
                    CreatorId = await _memberService.GetCurrentMemberIdAsync(),
                    MentionedUserIds = mentionIds,
                    Title = social.Description.StripHtml().TrimByWordEnd(maxTitleLength),
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.Social
                });
            }
        }
    }
}