using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extentions;
using uIntra.Core.User;

namespace uIntra.Users
{
    public class RoleServiceBase : IRoleService
    {
        public virtual IEnumerable<IRole> GetAll()
        {
            var roles = new List<IRole>();

            foreach (IntranetRolesEnum enumRole in Enum.GetValues(typeof(IntranetRolesEnum)))
            {
                roles.Add(new Role
                {
                    Name = enumRole.ToRoleName(),
                    Priority = enumRole.GetHashCode()
                });
            }

            return roles;
        }

        public virtual IRole GetByName(string name)
        {
            IntranetRolesEnum role;
            if (Enum.TryParse(name, out role))
            {
                return new Role
                {
                    Name = role.ToRoleName(),
                    Priority = role.GetHashCode()
                };
            }

            throw new Exception($"Can't map group name {name} to IntranetUserRole");
        }

        public virtual IRole GetDefaultRole()
        {
            return new Role
            {
                Name = IntranetRolesEnum.UiUser.ToRoleName(),
                Priority = IntranetRolesEnum.UiUser.GetHashCode()
            };
        }

        public virtual IRole GetHightestRole(IEnumerable<string> roleNames)
        {
            var roles = GetRoleByNames(roleNames).OrderBy(x => x.Priority);
            return roles.First();
        }

        private IEnumerable<IRole> GetRoleByNames(IEnumerable<string> names)
        {
            return names.Select(GetByName).ToList();
        }
    }
}
