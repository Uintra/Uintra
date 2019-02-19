using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.TypeProviders
{
    public class IntranetMemberGroupProvider: IIntranetMemberGroupProvider
    {
        private readonly IIntranetMemberGroupService _groupService;

        public IntranetMemberGroup this[int typeId] => IntTypeDictionary[typeId];

        public IntranetMemberGroup this[string name] => StringTypeDictionary[name];

        //public IntranetMemberGroup[] All { get; }

        public IntranetMemberGroup[] All { get { return _groupService.GetAll(); } }

        public IDictionary<int, IntranetMemberGroup> IntTypeDictionary { get { return All.ToDictionary(role => role.Id); } }

        public IDictionary<string, IntranetMemberGroup> StringTypeDictionary { get { return All.ToDictionary(role => role.Name); } }


        public IntranetMemberGroupProvider(IIntranetMemberGroupService groupService)
        {
            //All = groupService.GetAll();
            _groupService = groupService;

            //IntTypeDictionary = All.ToDictionary(role => role.Id);
            //StringTypeDictionary = All.ToDictionary(role => role.Name);
        }
    }
}
