using System.Collections.Generic;
using System.Linq;
using Uintra.Features.Permissions.Interfaces;
using Uintra.Features.Permissions.Models;

namespace Uintra.Features.Permissions.TypeProviders
{
    public class IntranetMemberGroupProvider : IIntranetMemberGroupProvider
    {
        private readonly IIntranetMemberGroupService _groupService;

        public IntranetMemberGroup this[int typeId] => IntTypeDictionary[typeId];

        public IntranetMemberGroup this[string name] => StringTypeDictionary[name];

        public IEnumerable<IntranetMemberGroup> All => _groupService.GetAll();

        public IDictionary<int, IntranetMemberGroup> IntTypeDictionary => All.ToDictionary(group => group.Id);

        public IDictionary<string, IntranetMemberGroup> StringTypeDictionary => All.ToDictionary(group => group.Name);

        public IntranetMemberGroupProvider(IIntranetMemberGroupService groupService)
        {
            _groupService = groupService;
        }
    }
}