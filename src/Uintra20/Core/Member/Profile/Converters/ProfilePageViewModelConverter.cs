using Compent.Extensions;
using UBaseline.Core.RequestContext;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Profile.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Core.User;
using Uintra20.Features.Links;
using Uintra20.Features.Media.Enums;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.Media.Images.Helpers.Contracts;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Member.Profile.Converters
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