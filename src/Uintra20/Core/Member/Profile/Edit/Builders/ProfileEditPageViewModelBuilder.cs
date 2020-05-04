using Compent.Extensions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Profile.Edit.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Media.Images.Helpers.Contracts;
using Uintra20.Features.Media.Strategies.Preset;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Member.Profile.Edit.Builders
{
    public class ProfileEditPageViewModelBuilder : IProfileEditPageViewModelBuilder
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly IImageHelper _imageHelper;

        private ProfileEditPageViewModel EditPageViewModel { get; set; }

        public ProfileEditPageViewModelBuilder(
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

        public ProfileEditPageViewModelBuilder SetModel(ProfileEditPageViewModel model)
        {
            EditPageViewModel = model;

            return this;
        }

        public ProfileEditPageViewModel Build()
        {
            return EditPageViewModel;
        }

        public ProfileEditPageViewModelBuilder BuildTags()
        {
            var member = _intranetMemberService.GetCurrentMember();

            EditPageViewModel.Tags = _userTagService.Get(member.Id);

            return this;
        }

        public ProfileEditPageViewModelBuilder BuildAvailableTags()
        {
            EditPageViewModel.AvailableTags = _userTagProvider.GetAll();

            return this;
        }

        public ProfileEditPageViewModelBuilder BuildMemberSettings()
        {
            var member = _intranetMemberService.GetCurrentMember();

            EditPageViewModel.Profile.MemberNotifierSettings = _memberNotifiersSettingsService.GetForMember(member.Id);

            return this;
        }

        public ProfileEditPageViewModelBuilder BuildPhoto()
        {
            if (EditPageViewModel.Profile.Photo.IsNullOrEmpty()) return this;

            EditPageViewModel.Profile.Photo =
                _imageHelper.GetImageWithResize(
                    EditPageViewModel.Profile.Photo,
                    PresetStrategies.ForMemberProfile.ThumbnailPreset);

            return this;
        }

        public ProfileEditPageViewModelBuilder BuildMemberProfile()
        {
            var member = _intranetMemberService.GetCurrentMember();

            EditPageViewModel.Profile = member.Map<ProfileEditViewModel>();

            return this;
        }
    }
}