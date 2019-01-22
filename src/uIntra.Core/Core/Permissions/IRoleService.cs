using LanguageExt;
using Uintra.Core.Permissions.Models;
using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions
{
    public interface IRoleService
    {
        Role[] GetAll();
        void Add(string name);
        void Remove(Role role);
    }
}