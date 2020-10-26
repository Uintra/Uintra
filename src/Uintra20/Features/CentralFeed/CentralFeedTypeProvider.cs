using System;
using Uintra20.Core.Feed;
using Uintra20.Infrastructure.TypeProviders;

namespace Uintra20.Features.CentralFeed
{
    public class CentralFeedTypeProvider : EnumTypeProviderBase, IFeedTypeProvider
    {
        public CentralFeedTypeProvider(params Type[] enums) : base(enums)
        {

        }
    }
}
