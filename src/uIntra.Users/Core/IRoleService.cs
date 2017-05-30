using System.Collections.Generic;
using uIntra.Core.User;

namespace uIntra.Users
{
    public interface IRoleService
    {
        IRole GetDefaultRole();
        IRole Get(string name);
        IRole GetActualRole(IEnumerable<string> roleNames);
        IEnumerable<IRole> GetAll();
    }
}