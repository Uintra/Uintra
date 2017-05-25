using uCommunity.Core.ApplicationSettings;
using uCommunity.Core.Web;

namespace Compent.uIntra.Controllers
{
    public class PinActivityController:PinActivityControllerBase
    {
        public PinActivityController(IApplicationSettings applicationSettings) : base(applicationSettings)
        {
        }
    }
}