using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Uintra20.Core.Activity;
using Uintra20.Core.Bulletins;
using Uintra20.Core.Bulletins.Web;
using Uintra20.Core.Extensions;
using Uintra20.Core.Feed;
using Uintra20.Core.Groups;
using Uintra20.Core.Links;
using Uintra20.Core.Media;
using Uintra20.Core.Navigation;
using Uintra20.Core.TypeProviders;
using Uintra20.Core.User;
using Uintra20.Core.UserTags;

namespace Uintra20.Controllers
{
    public class BulletinsController : BulletinsControllerBase
    {
        //protected override string DetailsViewPath => "~/Views/Bulletins/DetailsView.cshtml";
        //protected override string ItemViewPath => "~/Views/Bulletins/ItemView.cshtml";
        //protected override string EditViewPath => "~/Views/Bulletins/EditView.cshtml";
        //protected override string CreationFormViewPath { get; } = "~/Views/Bulletins/CreationForm.cshtml";
        //protected override string ItemHeaderViewPath { get; } = "~/Views/Bulletins/ItemHeader.cshtml";

        private readonly IBulletinsService<Bulletin> _bulletinsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IMyLinksService _myLinksService;
        private readonly IGroupActivityService _groupActivityService;
        private readonly IActivityTagsHelper _activityTagsHelper;
        private readonly IMentionService _mentionService;
        private readonly IActivityLinkService _activityLinkService;

        public BulletinsController(
            IBulletinsService<Bulletin> bulletinsService,
            IBulletinsService<BulletinBase> bulletinsBaseService,//TODO: Refactor
            IMediaHelper mediaHelper,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IActivityTypeProvider activityTypeProvider,
            IMyLinksService myLinksService,
            IGroupActivityService groupActivityService,
            IActivityTagsHelper activityTagsHelper,
            //IContextTypeProvider contextTypeProvider,
            IMentionService mentionService,
            IActivityLinkService activityLinkService)
            : base(bulletinsBaseService, mediaHelper, intranetMemberService, activityTypeProvider)
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
        public BulletinCreationResultModel CreateExtended(BulletinExtendedCreateModel model)
        {
            return Create(model);
        }

        //protected override BulletinCreateModel GetCreateFormModel(IActivityCreateLinks links)
        //{
        //    var model = base.GetCreateFormModel(links);
        //    var extendedModel = model.Map<BulletinExtendedCreateModel>();
        //    return extendedModel;
        //}

        [HttpPut]
        public HttpResponseMessage EditExtended(BulletinExtendedEditModel model)
        {
            return Edit(model);
        }

        //[HttpGet]
        //public BulletinExtendedItemViewModel FeedItem(Bulletin item, ActivityFeedOptionsWithGroups options)
        //{
        //    BulletinExtendedItemViewModel extendedModel = GetItemViewModel(item, options);
        //    return extendedModel;
        //}

        //[HttpGet]
        //public BulletinPreviewViewModel PreviewItem(Bulletin item, ActivityLinks links)
        //{
        //    BulletinPreviewViewModel viewModel = GetPreviewViewModel(item, links);
        //    return viewModel;
        //}

        //protected override BulletinEditModel GetEditViewModel(BulletinBase bulletin, ActivityLinks links)
        //{
        //    var baseModel = base.GetEditViewModel(bulletin, links);
        //    var extendedModel = baseModel.Map<BulletinExtendedEditModel>();
        //    return extendedModel;
        //}
        
        protected override BulletinViewModel GetViewModel(BulletinBase bulletin, ActivityFeedOptions options)
        {
            var extendedBullet = (Bulletin)bulletin;
            var extendedModel = base.GetViewModel(bulletin, options).Map<BulletinExtendedViewModel>();
            extendedModel = Mapper.Map(extendedBullet, extendedModel);
            return extendedModel;
        }

        //private BulletinExtendedItemViewModel GetItemViewModel(Bulletin item, ActivityFeedOptionsWithGroups options)
        //{
        //    var model = GetItemViewModel(item, options.Links);
        //    var extendedModel = model.Map<BulletinExtendedItemViewModel>();

        //    extendedModel.HeaderInfo = model.HeaderInfo.Map<ExtendedItemHeaderViewModel>();
        //    extendedModel.HeaderInfo.GroupInfo = options.GroupInfo;

        //    extendedModel.LikesInfo = item;
        //    extendedModel.LikesInfo.IsReadOnly = options.IsReadOnly;
        //    extendedModel.IsReadOnly = options.IsReadOnly;
        //    return extendedModel;
        //}

        protected override void OnBulletinEdited(BulletinBase bulletin, BulletinEditModel model)
        {
            if (model is BulletinExtendedEditModel extendedModel)
            {
                _activityTagsHelper.ReplaceTags(bulletin.Id, extendedModel.TagIdsData);
            }

            ResolveMentions(model.Description, bulletin);
        }

        protected override void OnBulletinDeleted(Guid id)
        {
            _myLinksService.DeleteByActivityId(id);
        }

        protected override void OnBulletinCreated(BulletinBase bulletin, BulletinCreateModel model)
        {
            base.OnBulletinCreated(bulletin, model);
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

        private void ResolveMentions(string text, BulletinBase bulletin)
        {
            var mentionIds = _mentionService.GetMentions(text).ToList();

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
    }
}