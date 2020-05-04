using Uintra20.Core.Member.Profile.Edit.Models;

namespace Uintra20.Core.Member.Profile.Edit.Builders
{
    public interface IProfileEditPageViewModelBuilder
    {
        ProfileEditPageViewModelBuilder SetModel(ProfileEditPageViewModel model);
        ProfileEditPageViewModel Build();
        ProfileEditPageViewModelBuilder BuildTags();
        ProfileEditPageViewModelBuilder BuildAvailableTags();
        ProfileEditPageViewModelBuilder BuildMemberSettings();
        ProfileEditPageViewModelBuilder BuildPhoto();
        ProfileEditPageViewModelBuilder BuildMemberProfile();
    }
}