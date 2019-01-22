using System.Collections.Generic;
using Uintra.Core.Permissions.Models;

namespace Uintra.Core.Permissions.TypeProviders
{
    public interface IRoleTypeProvider
    {
        Role this[int typeId] { get; }

        Role this[string name] { get; }

        Role[] All { get; }

        IDictionary<int, Role> IntTypeDictionary { get; }

        IDictionary<string, Role> StringTypeDictionary { get; }
    }
}