using System;
using Uintra.Core.Feed;
using Uintra.Infrastructure.TypeProviders;

namespace Uintra.Features.CentralFeed
{
    public class CentralFeedTypeProvider : EnumTypeProviderBase, IFeedTypeProvider
    {
        public CentralFeedTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
