using System;
using System.Linq;
using System.Web.Mvc;
using uIntra.Core.BrowserCompatibility;
using uIntra.Core.BrowserCompatibility.Models;
using uIntra.Core.Extentions;
using Umbraco.Web.Mvc;

namespace uIntra.Core.Web
{
    public abstract class BrowserCompatibilityControllerBase : SurfaceController
    {
        private const string BrowserCompatibilityCookieName = "browserCompatibilityCookie";
        private readonly IBrowserCompatibilityConfigurationSection browserCompatibilityConfiguration;
        private readonly ICookieProvider cookieProvider;

        protected virtual string BrowserCompatibilityNotificationViewPath { get; } = "~/App_Plugins/Core/BrowserCompatibility/BrowserCompatibilityNotificationView.cshtml";

        protected BrowserCompatibilityControllerBase(IBrowserCompatibilityConfigurationSection browserCompatibilityConfiguration, ICookieProvider cookieProvider)
        {
            this.browserCompatibilityConfiguration = browserCompatibilityConfiguration;
            this.cookieProvider = cookieProvider;
        }

        [HttpGet]
        public ActionResult BrowserCompatibility()
        {
            return PartialView(BrowserCompatibilityNotificationViewPath, GetBrowserCompatibilityModel());
        }

        protected virtual BrowserCompatibilityModel GetBrowserCompatibilityModel()
        {
            BrowserCompatibilityModel browserCompatibilityCookieValue;
            var compatibilityCookie = cookieProvider.Get(BrowserCompatibilityCookieName);

            if (string.IsNullOrEmpty(compatibilityCookie?.Value))
            {
                browserCompatibilityCookieValue = new BrowserCompatibilityModel
                {
                    BrowserSupported = IsBrowserSupported(),
                    ShowNotification = true
                };
            }
            else
            {
                browserCompatibilityCookieValue = compatibilityCookie.Value.Deserialize<BrowserCompatibilityModel>();
                browserCompatibilityCookieValue.BrowserSupported = IsBrowserSupported();
            }

            cookieProvider.Save(BrowserCompatibilityCookieName, browserCompatibilityCookieValue.ToJson(), DateTime.Now.AddYears(1));

            return browserCompatibilityCookieValue;
        }

        [System.Web.Http.HttpPost]
        public void DisableBrowserCompatibilityNotification()
        {
            var compatibilityCookie = cookieProvider.Get(BrowserCompatibilityCookieName);

            if (string.IsNullOrEmpty(compatibilityCookie?.Value))
            {
                return;
            }

            var browserCompatibilityCookieValue = compatibilityCookie.Value.Deserialize<BrowserCompatibilityModel>();
            browserCompatibilityCookieValue.ShowNotification = false;
            cookieProvider.Save(BrowserCompatibilityCookieName, browserCompatibilityCookieValue.ToJson(), DateTime.Now.AddYears(1));
        }

        protected virtual bool IsBrowserSupported()
        {
            var supportableBrowsers = browserCompatibilityConfiguration.Browsers;

            var currentBrowser = ControllerContext.HttpContext.Request.Browser;

            var supportableBrowser = supportableBrowsers.FirstOrDefault(b => String.Equals(b.Name, currentBrowser.Browser, StringComparison.OrdinalIgnoreCase));

            if (supportableBrowser == null)
            {
                return false;
            }

            var currentBrowserVersion = new Version(currentBrowser.Version);
            var supportableBrowserVersion = new Version(supportableBrowser.Version);

            return currentBrowserVersion >= supportableBrowserVersion;
        }
    }
}
