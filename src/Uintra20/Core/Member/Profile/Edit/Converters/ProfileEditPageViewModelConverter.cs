using Compent.Extensions;
using UBaseline.Core.Node;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Profile.Edit.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Media.Images.Helpers.Contracts;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Member.Profile.Edit.Converters
{
    public class ProfileEditPageViewModelConverter :
        INodeViewModelConverter<ProfileEditPageModel, ProfileEditPageViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IImageHelper _imageHelper;

        public ProfileEditPageViewModelConverter(
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IUserTagService userTagService,
            IUserTagProvider userTagProvider,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            IImageHelper imageHelper)
        {
            _intranetMemberService = intranetMemberService;
            _userTagService = userTagService;
            _userTagProvider = userTagProvider;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _imageHelper = imageHelper;
        }

        public void Map(
            ProfileEditPageModel node,
            ProfileEditPageViewModel viewModel)
        {
            var member = _intranetMemberService.GetCurrentMember();

            viewModel.Profile = member.Map<ProfileEditViewModel>();
            viewModel.Tags = _userTagService.Get(member.Id);
            viewModel.AvailableTags = _userTagProvider.GetAll();
            viewModel.Profile.MemberNotifierSettings = _memberNotifiersSettingsService.GetForMember(member.Id);
            viewModel.Profile.Photo = MapPhoto(viewModel.Profile.Photo);
        }

        public string MapPhoto(string photo)
        {
            if (photo.IsNullOrEmpty()) return photo;

            return _imageHelper.GetImageWithResize(photo, PresetStrategies.ForMemberProfile.ThumbnailPreset);
        }
    }
}