using System;

namespace Uintra.Core.TypeProviders
{
    public class ActivityTypeProvider : EnumTypeProviderBase, IActivityTypeProvider
    {
        public ActivityTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
