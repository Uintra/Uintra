using UBaseline.Core.Node;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Profile.Edit.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Member.Profile.Edit.Converters
{
    public class ProfileEditPageViewModelConverter :
        INodeViewModelConverter<ProfileEditPageModel, ProfileEditPageViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public ProfileEditPageViewModelConverter(
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _intranetMemberService = intranetMemberService;
        }

        public void Map(
            ProfileEditPageModel node,
            ProfileEditPageViewModel viewModel)
        {
            var member = _intranetMemberService.GetCurrentMember();

            viewModel.Profile = member.Map<ProfileEditModel>();
        }
    }
}