using System;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Features.Permissions.TypeProviders
{
    public class PermissionActionTypeProvider : EnumTypeProviderBase, IPermissionActionTypeProvider
    {
        public PermissionActionTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}