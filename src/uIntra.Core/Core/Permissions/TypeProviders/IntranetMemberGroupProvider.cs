using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.TypeProviders
{
    public class IntranetMemberGroupProvider: IIntranetMemberGroupProvider
    {
        public IntranetMemberGroup this[int typeId] => IntTypeDictionary[typeId];

        public IntranetMemberGroup this[string name] => StringTypeDictionary[name];

        public IntranetMemberGroup[] All { get; }

        public IDictionary<int, IntranetMemberGroup> IntTypeDictionary { get; }

        public IDictionary<string, IntranetMemberGroup> StringTypeDictionary { get; }


        public IntranetMemberGroupProvider(IIntranetMemberGroupService groupService)
        {
            All = groupService.GetAll();

            IntTypeDictionary = All.ToDictionary(role => role.Id);
            StringTypeDictionary = All.ToDictionary(role => role.Name);
        }
    }
}
