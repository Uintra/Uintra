using System;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.Permissions.TypeProviders
{
    public interface IIntranetActionTypeProvider: IEnumTypeProvider
    {
        Enum [] ActivityActions { get; }
    }
}