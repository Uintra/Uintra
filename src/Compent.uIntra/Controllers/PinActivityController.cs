using uCommunity.Core.ApplicationSettings;
using uCommunity.Core.Web;

namespace Compent.uCommunity.Controllers
{
    public class PinActivityController:PinActivityControllerBase
    {
        public PinActivityController(IApplicationSettings applicationSettings) : base(applicationSettings)
        {
        }
    }
}