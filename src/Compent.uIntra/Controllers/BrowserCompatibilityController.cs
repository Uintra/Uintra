using uIntra.Core;
using uIntra.Core.BrowserCompatibility;
using uIntra.Core.Web;

namespace Compent.uIntra.Controllers
{
    public class BrowserCompatibilityController : BrowserCompatibilityControllerBase
    {        
        public BrowserCompatibilityController(IBrowserCompatibilityConfigurationSection browserCompatibilityConfiguration, ICookieProvider cookieProvider) : base(browserCompatibilityConfiguration, cookieProvider)
        {
        }
    }
}