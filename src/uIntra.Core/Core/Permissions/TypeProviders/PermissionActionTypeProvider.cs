using System;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.Permissions.TypeProviders
{
    public class PermissionActionTypeProvider : EnumTypeProviderBase<PermissionActionEnum>, IPermissionActionTypeProvider
    {
        public PermissionActionTypeProvider()
        {
            ActivityActions = new Enum []
            {
                PermissionActionEnum.View,
                PermissionActionEnum.Create,
                PermissionActionEnum.Edit,
                PermissionActionEnum.Delete,
                PermissionActionEnum.CanEditOwner
            };
        }

        public Enum[] ActivityActions { get; }
    }
}
