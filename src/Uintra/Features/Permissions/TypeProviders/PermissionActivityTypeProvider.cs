using System;
using Uintra.Infrastructure.TypeProviders;

namespace Uintra.Features.Permissions.TypeProviders
{
    public class PermissionActivityTypeProvider : EnumTypeProviderBase, IPermissionResourceTypeProvider
    {
        public PermissionActivityTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}