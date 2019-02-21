using System;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.Permissions.TypeProviders
{
    public interface IPermissionActionTypeProvider: IEnumTypeProvider
    {
        Enum [] ActivityActions { get; }
    }
}