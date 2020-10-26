using System;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Features.Permissions.TypeProviders
{
    public class PermissionActivityTypeProvider : EnumTypeProviderBase, IPermissionResourceTypeProvider
    {
        public PermissionActivityTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}