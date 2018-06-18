using System.Web;
using Uintra.Core;
using Uintra.Core.BrowserCompatibility;
using Uintra.Core.Web;

namespace Compent.Uintra.Controllers
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