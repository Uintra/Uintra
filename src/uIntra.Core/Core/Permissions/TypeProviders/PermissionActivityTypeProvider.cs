using System;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.Permissions.TypeProviders
{
    public class PermissionActivityTypeProvider : EnumTypeProviderBase, IPermissionResourceTypeProvider
    {
        public PermissionActivityTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
