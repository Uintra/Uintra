using UBaseline.Core.Node;
using Uintra20.Core.Member.Profile.Edit.Builders;
using Uintra20.Core.Member.Profile.Edit.Models;

namespace Uintra20.Core.Member.Profile.Edit.Converters
{
    public class ProfileEditPageViewModelConverter :
        INodeViewModelConverter<ProfileEditPageModel, ProfileEditPageViewModel>
    {
        private readonly IProfileEditPageViewModelBuilder _profileEditPageViewModelBuilder;

        public ProfileEditPageViewModelConverter(IProfileEditPageViewModelBuilder profileEditPageViewModelBuilder) =>
            _profileEditPageViewModelBuilder = profileEditPageViewModelBuilder;

        public void Map(
            ProfileEditPageModel profileEditPageModel,
            ProfileEditPageViewModel profileEditPageViewModel)
        {
            _profileEditPageViewModelBuilder
                .SetModel(profileEditPageViewModel)
                .BuildMemberProfile()
                .BuildPhoto()
                .BuildTags()
                .BuildAvailableTags()
                .BuildMemberSettings()
                .Build();
        }
    }
}
