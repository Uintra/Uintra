using System;
using Uintra.Core.Activity;
using Uintra.Core.TypeProviders;

namespace Uintra.Core.Permissions.TypeProviders
{
    public class IntranetActionTypeProvider : EnumTypeProviderBase<IntranetActionEnum>, IIntranetActionTypeProvider
    {
        public IntranetActionTypeProvider()
        {
            ActivityActions = new Enum []
            {
                IntranetActionEnum.View,
                IntranetActionEnum.Create,
                IntranetActionEnum.Edit,
                IntranetActionEnum.Delete
            };
        }

        public Enum[] ActivityActions { get; }
    }
}
