using System;
using System.Web;
using UBaseline.Core.Node;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Profile.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Tagging.UserTags.Services;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Core.Member.Profile.Converters
{
    public class ProfilePageViewModelConverter : 
        INodeViewModelConverter<ProfilePageModel, ProfilePageViewModel>
    {
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly IUserTagService _userTagService;
        private readonly IUserTagProvider _userTagProvider;

        public ProfilePageViewModelConverter(
            IIntranetMemberService<IntranetMember> memberService, 
            IUserTagService userTagService, 
            IUserTagProvider userTagProvider)
        {
            _memberService = memberService;
            _userTagService = userTagService;
            _userTagProvider = userTagProvider;
        }

        public void Map(
            ProfilePageModel node, 
            ProfilePageViewModel viewModel)
        {
            var id = HttpContext.Current.Request.GetRequestQueryValue("id");

            if (!Guid.TryParse(id, out var parseId))  return;

            var member = _memberService.Get(parseId);

            viewModel.Profile = member.Map<ProfileViewModel>();
            viewModel.Tags = _userTagService.Get(member.Id);
        }
    }
}