using Compent.Shared.Extensions.Bcl;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
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
    public class BulletinsController
    {
        private readonly IBulletinsService<Bulletin> _bulletinsService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IMyLinksService _myLinksService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IMentionService _mentionService;
        private readonly IActivityLinkService _activityLinkService;

        public BulletinsController(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetMemberService<IntranetMember> memberService,
            IMyLinksService myLinksService,
            IGroupActivityService groupActivityService,
            IActivityTagsHelper activityTagsHelper,
            IMentionService mentionService,
            IActivityLinkService activityLinkService)
        {
            _bulletinsService = bulletinsService;
            _mediaHelper = mediaHelper;
            _memberService = memberService;
            _myLinksService = myLinksService;
            _groupActivityService = groupActivityService;
            _activityTagsHelper = activityTagsHelper;
            _mentionService = mentionService;
            _activityLinkService = activityLinkService;
        }

        [HttpPost]
        public async Task<BulletinCreationResultModel> CreateExtended(BulletinExtendedCreateModel model)
        {
            var result = new BulletinCreationResultModel();

            var bulletin = MapToBulletin(model);
            var createdBulletinId = await _bulletinsService.CreateAsync(bulletin);
            bulletin.Id = createdBulletinId;
            await OnBulletinCreatedAsync(bulletin, model);

            result.Id = createdBulletinId;
            result.IsSuccess = true;

            return result;
        }

        [HttpPut]
        public async Task<HttpResponseMessage> EditExtended(BulletinExtendedEditModel editModel)
        {
            var bulletin = MapToBulletin(editModel);
            await _bulletinsService.SaveAsync(bulletin);
            await OnBulletinEditedAsync(bulletin, editModel);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            await _bulletinsService.DeleteAsync(id);
            await OnBulletinDeletedAsync(id);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private BulletinBase MapToBulletin(BulletinCreateModel model)
        {
            var bulletin = model.Map<BulletinBase>();
            bulletin.PublishDate = DateTime.UtcNow;
            bulletin.CreatorId = bulletin.OwnerId = _memberService.GetCurrentMemberId();

            if (model.NewMedia.HasValue())
            {
                bulletin.MediaIds = _mediaHelper.CreateMedia(model);
            }

            return bulletin;
        }

        private BulletinBase MapToBulletin(BulletinEditModel editModel)
        {
            var bulletin = _bulletinsService.Get(editModel.Id);
            //bulletin = editModel.Map(bulletin);
            bulletin.MediaIds = bulletin.MediaIds.Concat(_mediaHelper.CreateMedia(editModel));

            return bulletin;
        }

        private void OnBulletinEdited(BulletinBase bulletin, BulletinEditModel model)
        {
            if (model is BulletinExtendedEditModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(bulletin.Id, extendedModel.TagIdsData);
            }

            ResolveMentions(model.Description, bulletin);
        }

        private async Task OnBulletinEditedAsync(BulletinBase bulletin, BulletinEditModel model)
        {
            if (model is BulletinExtendedEditModel extendedModel)
            {
                await _activityTagsHelper.ReplaceTagsAsync(bulletin.Id, extendedModel.TagIdsData);
            }

            await ResolveMentionsAsync(model.Description, bulletin);
        }

        private void OnBulletinDeleted(Guid id)
        {
            _myLinksService.DeleteByActivityId(id);
        }

        private async Task OnBulletinDeletedAsync(Guid id)
        {
            await _myLinksService.DeleteByActivityIdAsync(id);
        }

        private void OnBulletinCreated(BulletinBase bulletin, BulletinCreateModel model)
        {
            var groupId = HttpContext.Current.Request.QueryString.GetGroupIdOrNone();

            if (groupId.HasValue)
                _groupActivityService.AddRelation(groupId.Value, bulletin.Id);

            var extendedBulletin = _bulletinsService.Get(bulletin.Id);
            extendedBulletin.GroupId = groupId;

            if (model is BulletinExtendedCreateModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(bulletin.Id, extendedModel.TagIdsData);
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return;
            }
            ResolveMentions(model.Description, bulletin);
        }

        private async Task OnBulletinCreatedAsync(BulletinBase bulletin, BulletinCreateModel model)
        {
            var groupId = HttpContext.Current.Request.QueryString.GetGroupIdOrNone();

            if (groupId.HasValue)
                await _groupActivityService.AddRelationAsync(groupId.Value, bulletin.Id);

            var extendedBulletin = _bulletinsService.Get(bulletin.Id);
            extendedBulletin.GroupId = groupId;

            if (model is BulletinExtendedCreateModel extendedModel)
            {
                await _activityTagsHelper.ReplaceTagsAsync(bulletin.Id, extendedModel.TagIdsData);
            }

            if (string.IsNullOrEmpty(model.Description))
            {
                return;
            }
            await ResolveMentionsAsync(model.Description, bulletin);
        }

        private void ResolveMentions(string text, BulletinBase bulletin)
        {
            var mentionIds = new Guid[] { };//_mentionService.GetMentions(text).ToList();//TODO: uncomment when mention service is ready

            if (mentionIds.Any())
            {
                var links = _activityLinkService.GetLinks(bulletin.Id);
                const int maxTitleLength = 100;
                _mentionService.ProcessMention(new MentionModel()
                {
                    MentionedSourceId = bulletin.Id,
                    CreatorId = _memberService.GetCurrentMemberId(),
                    MentionedUserIds = mentionIds,
                    Title = bulletin.Description.StripHtml().TrimByWordEnd(maxTitleLength),
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.Bulletins
                });

            }
        }

        private async Task ResolveMentionsAsync(string text, BulletinBase bulletin)
        {
            var mentionIds = new Guid[] { };//_mentionService.GetMentions(text).ToList();//TODO: uncomment when mention service is ready

            if (mentionIds.Any())
            {
                var links = await _activityLinkService.GetLinksAsync(bulletin.Id);
                const int maxTitleLength = 100;
                _mentionService.ProcessMention(new MentionModel()
                {
                    MentionedSourceId = bulletin.Id,
                    CreatorId = await _memberService.GetCurrentMemberIdAsync(),
                    MentionedUserIds = mentionIds,
                    Title = bulletin.Description.StripHtml().TrimByWordEnd(maxTitleLength),
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.Bulletins
                });

            }
        }
    }
}