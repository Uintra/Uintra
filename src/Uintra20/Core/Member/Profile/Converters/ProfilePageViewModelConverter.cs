using System;
using System.Threading.Tasks;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Profile.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Member.Profile.Converters
{
    public class ProfilePageViewModelConverter : 
        INodeViewModelConverter<ProfilePageModel, ProfilePageViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _memberService;

        public ProfilePageViewModelConverter(IIntranetMemberService<IntranetMember> memberService)
        {
            _memberService = memberService;
        }

        public void Map(
            ProfilePageModel node, 
            ProfilePageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetUbaselineQueryValue("id");

            if (!Guid.TryParse(id, out var parseId))  return;

            var member = _memberService.Get(parseId);

            viewModel.Profile = member.Map<ProfileViewModel>();

        }
    }
}