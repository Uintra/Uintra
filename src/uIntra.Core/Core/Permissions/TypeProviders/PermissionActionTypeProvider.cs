using System;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.Permissions.TypeProviders
{
    public class PermissionActionTypeProvider : EnumTypeProviderBase, IPermissionActionTypeProvider
    {
        public PermissionActionTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
