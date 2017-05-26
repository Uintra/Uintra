using System.Collections.Generic;
using uIntra.Core.User;

namespace uIntra.Users
{
    public interface IRoleService
    {
        IRole GetDefaultRole();
        IRole Get(string name);
        IRole GetHightestRole(IEnumerable<string> roleNames);
        IEnumerable<IRole> GetAll();
    }
}