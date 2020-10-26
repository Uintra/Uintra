using System;
using Uintra.Infrastructure.TypeProviders;

namespace Uintra.Features.Permissions.TypeProviders
{
    public class PermissionActionTypeProvider : EnumTypeProviderBase, IPermissionActionTypeProvider
    {
        public PermissionActionTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}