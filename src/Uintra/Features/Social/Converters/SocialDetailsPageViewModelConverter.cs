using System;
using System.Linq;
using System.Web;
using Compent.Extensions;
using UBaseline.Core.RequestContext;
using Uintra.Core.Activity.Models.Headers;
using Uintra.Core.Controls.LightboxGallery;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Features.Groups.Helpers;
using Uintra.Features.Groups.Services;
using Uintra.Features.Links;
using Uintra.Features.Media;
using Uintra.Features.Media.Helpers;
using Uintra.Features.Media.Strategies.Preset;
using Uintra.Features.Permissions;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Social.Models;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Features.Social.Converters
{
    public class SocialDetailsPageViewModelConverter :
        UintraRestrictedNodeViewModelConverter<SocialDetailsPageModel, SocialDetailsPageViewModel>
    {
        private readonly IFeedLinkService _feedLinkService;
        private readonly IUserTagService _userTagService;
        private readonly ISocialService<Entities.Social> _socialService;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly ILightboxHelper _lightboxHelper;
        private readonly IPermissionsService _permissionsService;
        private readonly IGroupHelper _groupHelper;
        private readonly IUBaselineRequestContext _context;
        private readonly IGroupService _groupService;
        public SocialDetailsPageViewModelConverter(
            IFeedLinkService feedLinkService,
            IIntranetMemberService<IntranetMember> memberService,
            IUserTagService userTagService,
            ISocialService<Entities.Social> socialsService,
            ILightboxHelper lightboxHelper,
            IPermissionsService permissionsService,
            IGroupHelper groupHelper,
            IErrorLinksService errorLinksService, 
            IUBaselineRequestContext context,
            IGroupService groupService)
            : base(errorLinksService)
        {
            _feedLinkService = feedLinkService;
            _userTagService = userTagService;
            _socialService = socialsService;
            _memberService = memberService;
            _lightboxHelper = lightboxHelper;
            _permissionsService = permissionsService;
            _groupHelper = groupHelper;
            _context = context;
            _groupService = groupService;
        }

        public override ConverterResponseModel MapViewModel(SocialDetailsPageModel node, SocialDetailsPageViewModel viewModel)
        {
            var id = _context.ParseQueryString("id").TryParseGuid();

            if (!id.HasValue) return NotFoundResult();

            var social = _socialService.Get(id.Value);

            if (social == null) return NotFoundResult();

            if (social.GroupId.HasValue)
            {
                var group = _groupService.Get(social.GroupId.Value);
                if (group != null && group.IsHidden)
                    return NotFoundResult();
            }

            if (!_permissionsService.Check(PermissionResourceTypeEnum.Social, PermissionActionEnum.View))
            {
                return ForbiddenResult();
            }

            var member = _memberService.GetCurrentMember();
            
            viewModel.Details = GetViewModel(social);
            viewModel.Tags = _userTagService.Get(id.Value);
            viewModel.CanEdit = _socialService.CanEdit(id.Value);
            viewModel.IsGroupMember = !social.GroupId.HasValue || member.GroupIds.Contains(social.GroupId.Value);

            var groupIdStr = HttpContext.Current.Request["groupId"];
            if (Guid.TryParse(groupIdStr, out var groupId) && social.GroupId == groupId)
            {
                viewModel.GroupHeader = _groupHelper.GetHeader(groupId);
            }

            return OkResult();
        }

        protected SocialExtendedViewModel GetViewModel(Entities.Social social)
        {
            var viewModel = social.Map<SocialViewModel>();

            viewModel.Media = MediaHelper.GetMediaUrls(social.MediaIds);

            viewModel.LightboxPreviewModel = _lightboxHelper.GetGalleryPreviewModel(social.MediaIds, PresetStrategies.ForActivityDetails);
            viewModel.CanEdit = _socialService.CanEdit(social);
            viewModel.Links = _feedLinkService.GetLinks(social.Id);
            viewModel.IsReadOnly = false;
            viewModel.HeaderInfo = social.Map<IntranetActivityDetailsHeaderViewModel>();
            viewModel.HeaderInfo.Dates = social.PublishDate.ToDateTimeFormat().ToEnumerable();
            viewModel.HeaderInfo.Owner = _memberService.Get(social).ToViewModel();
            viewModel.HeaderInfo.Links = _feedLinkService.GetLinks(social.Id);

            var extendedModel = viewModel.Map<SocialExtendedViewModel>();

            return extendedModel;
        }
    }
}