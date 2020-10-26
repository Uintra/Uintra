using System;

namespace Uintra.Infrastructure.TypeProviders
{
    public class ActivityTypeProvider : EnumTypeProviderBase, IActivityTypeProvider
    {
        public ActivityTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}