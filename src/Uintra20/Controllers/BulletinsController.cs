using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Uintra20.Attributes;
using Uintra20.Core.Activity;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.Bulletins.Models;
using Uintra20.Features.Bulletins.Web;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Navigation.Services;
using Uintra20.Features.Tagging.UserTags;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Controllers
{
    [ValidateModel]
    public class BulletinsController : BulletinsControllerBase
    {
        private readonly IBulletinsService<Bulletin> _bulletinsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IMyLinksService _myLinksService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IMentionService _mentionService;
        private readonly IActivityLinkService _activityLinkService;

        public BulletinsController(
            IBulletinsService<Bulletin> bulletinsService,
            IMediaHelper mediaHelper,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IMyLinksService myLinksService,
            IGroupActivityService groupActivityService,
            IActivityTagsHelper activityTagsHelper,
            IMentionService mentionService,
            IActivityLinkService activityLinkService)
            : base(bulletinsService, mediaHelper, intranetMemberService)
        {
            _bulletinsService = bulletinsService;
            _intranetMemberService = intranetMemberService;
            _myLinksService = myLinksService;
            _groupActivityService = groupActivityService;
            _activityTagsHelper = activityTagsHelper;
            _mentionService = mentionService;
            _activityLinkService = activityLinkService;
        }

        [HttpPost]
        public async Task<BulletinCreationResultModel> CreateExtended(BulletinExtendedCreateModel model)
        {
            return await Create(model);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> EditExtended(BulletinExtendedEditModel model)
        {
            return await Edit(model);
        }

        protected override void OnBulletinEdited(BulletinBase bulletin, BulletinEditModel model)
        {
            if (model is BulletinExtendedEditModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(bulletin.Id, extendedModel.TagIdsData);
            }

            ResolveMentions(model.Description, bulletin);
        }

        protected override async Task OnBulletinEditedAsync(BulletinBase bulletin, BulletinEditModel model)
        {
            if (model is BulletinExtendedEditModel extendedModel)
            {
                await _activityTagsHelper.ReplaceTagsAsync(bulletin.Id, extendedModel.TagIdsData);
            }

            await ResolveMentionsAsync(model.Description, bulletin);
        }

        protected override void OnBulletinDeleted(Guid id)
        {
            _myLinksService.DeleteByActivityId(id);
        }

        protected override async Task OnBulletinDeletedAsync(Guid id)
        {
            await _myLinksService.DeleteByActivityIdAsync(id);
        }

        protected override void OnBulletinCreated(BulletinBase bulletin, BulletinCreateModel model)
        {
            var groupId = HttpContext.Current.Request.QueryString.GetGroupIdOrNone();

            if(groupId.HasValue)
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

        protected override async Task OnBulletinCreatedAsync(BulletinBase bulletin, BulletinCreateModel model)
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
                    CreatorId = _intranetMemberService.GetCurrentMemberId(),
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
                    CreatorId = await _intranetMemberService.GetCurrentMemberIdAsync(),
                    MentionedUserIds = mentionIds,
                    Title = bulletin.Description.StripHtml().TrimByWordEnd(maxTitleLength),
                    Url = links.Details,
                    ActivityType = IntranetActivityTypeEnum.Bulletins
                });

            }
        }
    }
}