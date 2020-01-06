using Compent.Shared.Extensions.Bcl;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using UBaseline.Core.Controllers;
using Uintra20.Core.Activity;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.Bulletins.Models;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Navigation.Services;
using Uintra20.Features.Tagging.UserTags;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Controllers
{
    public class SocialController : UBaselineApiController
    {
        private readonly ISocialService<Social> _socialService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IMyLinksService _myLinksService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IMentionService _mentionService;
        private readonly IActivityLinkService _activityLinkService;

        public SocialController(
            ISocialService<Social> socialService,
            IMediaHelper mediaHelper,
            IIntranetMemberService<IntranetMember> memberService,
            IMyLinksService myLinksService,
            IGroupActivityService groupActivityService,
            IActivityTagsHelper activityTagsHelper,
            IMentionService mentionService,
            IActivityLinkService activityLinkService)
        {
            _socialService = socialService;
            _mediaHelper = mediaHelper;
            _memberService = memberService;
            _myLinksService = myLinksService;
            _groupActivityService = groupActivityService;
            _activityTagsHelper = activityTagsHelper;
            _mentionService = mentionService;
            _activityLinkService = activityLinkService;
        }

        [HttpPost]
        public async Task<SocialCreationResultModel> CreateExtended(SocialExtendedCreateModel model)
        {
            var result = new SocialCreationResultModel();

            var bulletin = MapToBulletin(model);
            var createdBulletinId = await _socialService.CreateAsync(bulletin);
            bulletin.Id = createdBulletinId;
            await OnBulletinCreatedAsync(bulletin, model);

            result.Id = createdBulletinId;
            result.IsSuccess = true;

            return result;
        }

        [HttpPut]
        public async Task<HttpResponseMessage> EditExtended(SocialExtendedEditModel editModel)
        {
            var bulletin = MapToBulletin(editModel);
            await _socialService.SaveAsync(bulletin);
            await OnBulletinEditedAsync(bulletin, editModel);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            await _socialService.DeleteAsync(id);
            await OnBulletinDeletedAsync(id);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private SocialBase MapToBulletin(SocialCreateModel model)
        {
            var bulletin = model.Map<SocialBase>();
            bulletin.PublishDate = DateTime.UtcNow;
            bulletin.CreatorId = bulletin.OwnerId = _memberService.GetCurrentMemberId();

            if (model.NewMedia.HasValue())
            {
                bulletin.MediaIds = _mediaHelper.CreateMedia(model);
            }

            return bulletin;
        }

        private SocialBase MapToBulletin(SocialEditModel editModel)
        {
            var bulletin = _socialService.Get(editModel.Id);
            //social = editModel.Map(social);
            bulletin.MediaIds = bulletin.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));

            return bulletin;
        }

        private void OnBulletinEdited(SocialBase social, SocialEditModel model)
        {
            if (model is SocialExtendedEditModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(social.Id, extendedModel.TagIdsData);
            }

            ResolveMentions(model.Description, social);
        }

        private async Task OnBulletinEditedAsync(SocialBase social, SocialEditModel model)
        {
            if (model is SocialExtendedEditModel extendedModel)
            {
                await _activityTagsHelper.ReplaceTagsAsync(social.Id, extendedModel.TagIdsData);
            }

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

            if (model is SocialExtendedCreateModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(social.Id, extendedModel.TagIdsData);
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return;
            }
            ResolveMentions(model.Description, social);
        }

        private async Task OnBulletinCreatedAsync(SocialBase social, SocialCreateModel model)
        {
            var groupId = HttpContext.Current.Request.QueryString.GetGroupIdOrNone();

            if (groupId.HasValue)
                await _groupActivityService.AddRelationAsync(groupId.Value, social.Id);

            var extendedBulletin = _socialService.Get(social.Id);
            extendedBulletin.GroupId = groupId;

            if (model is SocialExtendedCreateModel extendedModel)
            {
                await _activityTagsHelper.ReplaceTagsAsync(social.Id, extendedModel.TagIdsData);
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return;
            }
            await ResolveMentionsAsync(model.Description, social);
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
            var mentionIds = new Guid[] { };//_mentionService.GetMentions(text).ToList();//TODO: uncomment when mention service is ready

            if (mentionIds.Any())
            {
                var links = await _activityLinkService.GetLinksAsync(social.Id);
                const int maxTitleLength = 100;
                _mentionService.ProcessMention(new MentionModel()
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