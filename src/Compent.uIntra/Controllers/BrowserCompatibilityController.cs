using Uintra.Core;
using Uintra.Core.BrowserCompatibility;
using Uintra.Core.Web;

namespace Compent.Uintra.Controllers
{
    public class BrowserCompatibilityController : BrowserCompatibilityControllerBase
    {        
        public BrowserCompatibilityController(            
            IBrowserCompatibilityConfigurationSection browserCompatibilityConfiguration,
            ICookieProvider cookieProvider) 
            : base(browserCompatibilityConfiguration, cookieProvider)
        {
        }
    }
}