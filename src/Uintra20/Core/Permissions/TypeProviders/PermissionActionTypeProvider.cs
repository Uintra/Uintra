using System;
using Uintra20.Core.TypeProviders;

namespace Uintra20.Core.Permissions.TypeProviders
{
    public class PermissionActionTypeProvider : EnumTypeProviderBase, IPermissionActionTypeProvider
    {
        public PermissionActionTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}