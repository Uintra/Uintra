using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra20.Core.Permissions.Interfaces;
using Uintra20.Core.Permissions.Models;

namespace Uintra20.Core.Permissions.TypeProviders
{
    public class IntranetMemberGroupProvider : IIntranetMemberGroupProvider
    {
        private readonly IIntranetMemberGroupService _groupService;

        public IntranetMemberGroup this[int typeId] => IntTypeDictionary[typeId];

        public IntranetMemberGroup this[string name] => StringTypeDictionary[name];

        public IEnumerable<IntranetMemberGroup> All => _groupService.GetAll();

        public IDictionary<int, IntranetMemberGroup> IntTypeDictionary { get; }

        public IDictionary<string, IntranetMemberGroup> StringTypeDictionary { get; }

        public IntranetMemberGroupProvider(IIntranetMemberGroupService groupService)
        {
            _groupService = groupService;
            IntTypeDictionary = All.ToDictionary(group => group.Id);
            StringTypeDictionary = All.ToDictionary(group => group.Name);
        }
    }
}