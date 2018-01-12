using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using uIntra.Core.User;

namespace uIntra.Users
{
    public class RoleServiceBase : IRoleService
    {
        public virtual IRole GetDefaultRole()
        {
            return GetAll().Single(r => r.Name == IntranetRolesEnum.UiUser.ToString());
        }

        public virtual IRole Get(string name)
        {
            var role = GetAll().SingleOrDefault(r => r.Name == name);
            if (role == null)
            {
                throw new Exception($"Can't map group name {name} to IntranetUserRole");
            }

            return role;
        }

        public virtual IRole GetActualRole(IEnumerable<string> roleNames)
        {
            var roles = roleNames.Select(Get).OrderBy(x => x.Priority);
            var rolesList = roles.AsList();
            return rolesList.IsEmpty() ? GetDefaultRole() : rolesList.First();
        }

        public virtual IEnumerable<IRole> GetAll()
        {
            foreach (IntranetRolesEnum enumRole in Enum.GetValues(typeof(IntranetRolesEnum)))
            {
                yield return new Role
                {
                    Name = enumRole.ToString(),
                    Priority = enumRole.GetHashCode()
                };
            }
        }
    }
}
