using System.Collections.Generic;
using uIntra.Core.User;

namespace uIntra.Users
{
    public interface IRoleService
    {
        IEnumerable<IRole> GetAll();
        IRole GetByName(string name);
        IRole GetDefaultRole();
        IRole GetHightestRole(IEnumerable<string> roleNames);
    }
}