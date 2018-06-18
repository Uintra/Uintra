using System.Collections.Generic;
using Uintra.Core.User;

namespace Uintra.Users
{
    public interface IRoleService
    {
        IRole GetDefaultRole();
        IRole Get(string name);
        IRole GetActualRole(IEnumerable<string> roleNames);
        IEnumerable<IRole> GetAll();
    }
}