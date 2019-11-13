using System;

namespace Uintra20.Core.TypeProviders
{
    public class ActivityTypeProvider : EnumTypeProviderBase, IActivityTypeProvider
    {
        public ActivityTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}