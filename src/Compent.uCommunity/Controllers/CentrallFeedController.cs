using uCommunity.CentralFeed;
using uCommunity.CentralFeed.Core;
using uCommunity.CentralFeed.Web;

namespace Compent.uCommunity.Controllers
{
    public class CentralFeedController: CentralFeedControllerBase
    {
        public CentralFeedController(ICentralFeedService centralFeedService, 
            ICentralFeedContentHelper centralFeedContentHelper) 
            : base(centralFeedService, centralFeedContentHelper)
        {
        }
    }
}