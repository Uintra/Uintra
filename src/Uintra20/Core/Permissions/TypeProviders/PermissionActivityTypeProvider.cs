using System;
using Uintra20.Core.TypeProviders;

namespace Uintra20.Core.Permissions.TypeProviders
{
    public class PermissionActivityTypeProvider : EnumTypeProviderBase, IPermissionResourceTypeProvider
    {
        public PermissionActivityTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}