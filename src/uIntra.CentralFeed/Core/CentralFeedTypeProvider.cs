using System;
using Uintra.Core.TypeProviders;

namespace Uintra.CentralFeed
{
    public class CentralFeedTypeProvider : EnumTypeProviderBase, IFeedTypeProvider
    {
        public CentralFeedTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
