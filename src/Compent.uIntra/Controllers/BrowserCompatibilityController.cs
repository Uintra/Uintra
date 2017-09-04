using System.Web;
using uIntra.Core;
using uIntra.Core.BrowserCompatibility;
using uIntra.Core.Web;

namespace Compent.uIntra.Controllers
{
    public class BrowserCompatibilityController : BrowserCompatibilityControllerBase
    {        
        public BrowserCompatibilityController(
            HttpContext httpContext,
            IBrowserCompatibilityConfigurationSection browserCompatibilityConfiguration,
            ICookieProvider cookieProvider) 
            : base(httpContext, browserCompatibilityConfiguration, cookieProvider)
        {
        }
    }
}