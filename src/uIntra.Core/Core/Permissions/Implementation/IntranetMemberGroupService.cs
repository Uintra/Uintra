using System.Linq;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Umbraco.Core.Services;
using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions.Implementation
{
    public class IntranetMemberGroupService : IIntranetMemberGroupService
    {
        private readonly IMemberService _memberService;
        private readonly IMemberGroupService _memberGroupService;

        public IntranetMemberGroupService(IMemberGroupService memberGroupService, IMemberService memberService)
        {
            _memberGroupService = memberGroupService;
            _memberService = memberService;
        }

        public IntranetMemberGroup[] GetAll() =>
            _memberGroupService
                .GetAll()
                .Map<IntranetMemberGroup[]>();

        public IntranetMemberGroup[] GetForMember(int id)
        {
            var allGroups = _memberGroupService.GetAll();
            var groupNamesAssignedToMember = _memberService.GetAllRoles(id);

            var memberGroups = allGroups
                .Join(groupNamesAssignedToMember, group => group.Name, identity, (group, _) => group)
                .Map<IntranetMemberGroup[]>();

            return memberGroups;
        }
    }
}