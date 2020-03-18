using System;
using System.Web;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Profile.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Core.UbaselineModels.RestrictedNode;
using Uintra20.Core.User;
using Uintra20.Features.Links;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Helpers;
using Uintra20.Features.Media.Image.Helpers.Contracts;
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

        public ProfilePageViewModelConverter(
            IIntranetMemberService<IntranetMember> memberService, 
            IUserTagService userTagService,
            IIntranetUserContentProvider intranetUserContentProvider,
            IImageHelper imageHelper,
            IMediaHelper mediaHelper,
            IErrorLinksService errorLinksService)
        : base(errorLinksService)
        {
            _memberService = memberService;
            _userTagService = userTagService;
            _intranetUserContentProvider = intranetUserContentProvider;
            _imageHelper = imageHelper;
            _mediaHelper = mediaHelper;
        }

        public override ConverterResponseModel MapViewModel(ProfilePageModel node, ProfilePageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(id, out var parseId))
            {
                return NotFoundResult();
            }

            var member = _memberService.Get(parseId);

            if (member == null)
            {
                return NotFoundResult();
            }

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