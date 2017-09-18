using System.Collections.Generic;
using uIntra.Core.TypeProviders;

namespace uIntra.CentralFeed
{
    public interface ICentralFeedService : IFeedService
    {
        IEnumerable<IFeedItem> GetFeed(IIntranetType type);
        IEnumerable<IFeedItem> GetFeed();
    }
}
