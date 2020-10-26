using Compent.Extensions;
using UBaseline.Core.RequestContext;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Profile.Models;
using Uintra.Core.Member.Services;
using Uintra.Core.UbaselineModels.RestrictedNode;
using Uintra.Core.User;
using Uintra.Features.Links;
using Uintra.Features.Media.Enums;
using Uintra.Features.Media.Helpers;
using Uintra.Features.Media.Images.Helpers.Contracts;
using Uintra.Features.Media.Strategies.Preset;
using Uintra.Features.Tagging.UserTags.Services;
using Uintra.Infrastructure.Extensions;

namespace Uintra.Core.Member.Profile.Converters
{
    public class ProfilePageViewModelConverter : UintraRestrictedNodeViewModelConverter<ProfilePageModel, ProfilePageViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IUserTagService _userTagService;
        private readonly IIntranetUserContentProvider _intranetUserContentProvider;
        private readonly IImageHelper _imageHelper;
        private readonly IMediaHelper _mediaHelper;
        private readonly IUBaselineRequestContext _uBaselineRequestContext;

        public ProfilePageViewModelConverter(
            IIntranetMemberService<IntranetMember> memberService,
            IUserTagService userTagService,
            IIntranetUserContentProvider intranetUserContentProvider,
            IImageHelper imageHelper,
            IMediaHelper mediaHelper,
            IErrorLinksService errorLinksService,
            IUBaselineRequestContext uBaselineRequestContext)
        : base(errorLinksService)
        {
            _memberService = memberService;
            _userTagService = userTagService;
            _intranetUserContentProvider = intranetUserContentProvider;
            _imageHelper = imageHelper;
            _mediaHelper = mediaHelper;
            _uBaselineRequestContext = uBaselineRequestContext;
        }

        public override ConverterResponseModel MapViewModel(ProfilePageModel node, ProfilePageViewModel viewModel)
        {
            var id = _uBaselineRequestContext.ParseQueryString("id").TryParseGuid();

            if (!id.HasValue) return NotFoundResult();

            var member = _memberService.Get(id.Value);

            if (member == null) return NotFoundResult();

            var currentMemberId = _memberService.GetCurrentMemberId();

            if (member.Id == currentMemberId)
            {
                viewModel.EditProfileLink = _intranetUserContentProvider.GetEditPage().Url.ToLinkModel();
            }

            var mediaSettings = _mediaHelper.GetMediaFolderSettings(MediaFolderTypeEnum.MembersContent, true);

            viewModel.Profile = member.Map<ProfileViewModel>();
            viewModel.Profile.Photo = _imageHelper.GetImageWithResize(member.Photo, PresetStrategies.ForMemberProfile.ThumbnailPreset);
            viewModel.Profile.AllowedMediaExtensions = mediaSettings.AllowedMediaExtensions;
            viewModel.Tags = _userTagService.Get(member.Id);

            return OkResult();
        }
    }
}