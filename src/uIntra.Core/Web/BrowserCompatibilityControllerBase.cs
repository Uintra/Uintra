using System;
using System.Linq;
using System.Web.Http;
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

        protected BrowserCompatibilityControllerBase(IBrowserCompatibilityConfigurationSection browserCompatibilityConfiguration, ICookieProvider cookieProvider)
        {
            this.browserCompatibilityConfiguration = browserCompatibilityConfiguration;
            this.cookieProvider = cookieProvider;
        }

        [HttpGet]
        public bool ShowBrowserCompatibilityNotification()
        {
            var compatibilityCookie = cookieProvider.Get(BrowserCompatibilityCookieName);

            if (string.IsNullOrEmpty(compatibilityCookie?.Value))
            {
                return false;
            }

            var browserCompatibilityCookieValue = compatibilityCookie.Value.Deserialize<BrowserCompatibilityModel>();
            return browserCompatibilityCookieValue.ShowNotification &&
                   browserCompatibilityCookieValue.BrowserNotSupported;
        }

        [HttpPost]
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

        [HttpPost]
        public void CheckBrowserCompatibility()
        {
            var compatibilityCookie = cookieProvider.Get(BrowserCompatibilityCookieName);

            BrowserCompatibilityModel browserCompatibilityCookieValue;

            if (string.IsNullOrEmpty(compatibilityCookie?.Value))
            {
                browserCompatibilityCookieValue = new BrowserCompatibilityModel
                {
                    BrowserNotSupported = IsBrowserSupported(),
                    ShowNotification = true
                };
            }
            else
            {
                browserCompatibilityCookieValue = compatibilityCookie.Value.Deserialize<BrowserCompatibilityModel>();
                browserCompatibilityCookieValue.BrowserNotSupported = IsBrowserSupported();
                cookieProvider.Delete(BrowserCompatibilityCookieName);
            }

            cookieProvider.Save(BrowserCompatibilityCookieName, browserCompatibilityCookieValue.ToJson(), DateTime.Now.AddYears(1));
        }

        private bool IsBrowserSupported()
        {
            var supportableBrowsers = browserCompatibilityConfiguration.Browsers;

            var currentBrowser = HttpContext.Request.Browser;

            var supportableBrowser = supportableBrowsers.FirstOrDefault(b => String.Equals(b.Name, currentBrowser.Browser, StringComparison.OrdinalIgnoreCase));

            if (supportableBrowser == null)
            {
                return false;
            }

            var currentBrowserVersion = new Version(currentBrowser.Version);
            var supportableBrowserVersion = new Version(supportableBrowser.Version);

            return currentBrowserVersion.CompareTo(supportableBrowserVersion) > 0;
        }
    }
}
