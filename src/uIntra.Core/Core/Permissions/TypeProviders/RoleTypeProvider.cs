using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.TypeProviders
{
    public class RoleTypeProvider
    {
        public Role this[int typeId] => IntTypeDictionary[typeId];

        public Role this[string name] => StringTypeDictionary[name];

        public Role[] All { get; }

        public IDictionary<int, Role> IntTypeDictionary { get; }

        public IDictionary<string, Role> StringTypeDictionary { get; }


        public RoleTypeProvider(IRoleService roleService)
        {
            All = roleService.GetAll();

            IntTypeDictionary = All.ToDictionary(role => role.Id);
            StringTypeDictionary = All.ToDictionary(role => role.RoleName);
        }
    }
}
