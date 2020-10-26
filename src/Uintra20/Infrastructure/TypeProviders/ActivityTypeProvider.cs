using System;

namespace Uintra20.Infrastructure.TypeProviders
{
    public class ActivityTypeProvider : EnumTypeProviderBase, IActivityTypeProvider
    {
        public ActivityTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}