using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uintra.Core.BrowserCompatibility;
using Uintra.Core.BrowserCompatibility.Models;
using Uintra.Core.Extensions;
using Umbraco.Web.Mvc;

namespace Uintra.Core.Web
{
    public abstract class BrowserCompatibilityControllerBase : SurfaceController
    {
        private const string OperaUserAgentConstant = @"OPR/";
        private const string EdgeUserAgentConstant = @"Edge/";
        private const string ChromeUserAgentConstant = @"Chrome/";
        private const string BrowserCompatibilityCookieName = "browserCompatibilityCookie";
        private readonly IBrowserCompatibilityConfigurationSection _browserCompatibilityConfiguration;
        private readonly ICookieProvider _cookieProvider;

        protected virtual string BrowserCompatibilityNotificationViewPath { get; } = "~/App_Plugins/Core/BrowserCompatibility/BrowserCompatibilityNotificationView.cshtml";

        protected BrowserCompatibilityControllerBase(IBrowserCompatibilityConfigurationSection browserCompatibilityConfiguration, ICookieProvider cookieProvider)
        {
            this._browserCompatibilityConfiguration = browserCompatibilityConfiguration;
            this._cookieProvider = cookieProvider;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult BrowserCompatibility()
        {
            return PartialView(BrowserCompatibilityNotificationViewPath, GetBrowserCompatibilityModel());
        }

        protected virtual BrowserCompatibilityModel GetBrowserCompatibilityModel()
        {
            BrowserCompatibilityModel browserCompatibilityCookieValue;
            var compatibilityCookie = _cookieProvider.Get(BrowserCompatibilityCookieName);

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
                if (!browserCompatibilityCookieValue.BrowserSupported)
                {
                    browserCompatibilityCookieValue.BrowserSupported = IsBrowserSupported();
                }                
            }

            _cookieProvider.Save(BrowserCompatibilityCookieName, browserCompatibilityCookieValue.ToJson(), DateTime.Now.AddYears(1));

            return browserCompatibilityCookieValue;
        }

        [System.Web.Http.HttpPost]
        public void DisableBrowserCompatibilityNotification()
        {
            var compatibilityCookie = _cookieProvider.Get(BrowserCompatibilityCookieName);

            if (string.IsNullOrEmpty(compatibilityCookie?.Value))
            {
                return;
            }

            var browserCompatibilityCookieValue = compatibilityCookie.Value.Deserialize<BrowserCompatibilityModel>();
            browserCompatibilityCookieValue.ShowNotification = false;
            _cookieProvider.Save(BrowserCompatibilityCookieName, browserCompatibilityCookieValue.ToJson(), DateTime.Now.AddYears(1));
        }

        protected virtual bool IsBrowserSupported()
        {
            //check opera separatly because asp.net detect opera as chrome
            if (IsBrowser(OperaUserAgentConstant))
            {
                return CheckBrowserSeparatly(OperaUserAgentConstant, "Opera", GetBrowserVersion);
            }

            //check edge separatly because asp.net detect opera as chrome
            if (IsBrowser(EdgeUserAgentConstant))
            {
                return CheckBrowserSeparatly(EdgeUserAgentConstant, "Edge", GetBrowserVersion);
            }

            var currentBrowser = HttpContext.Request.Browser;

            //check chrome separatly because asp.net detect incorrect version of chrome
            if (currentBrowser.Browser == "Chrome")
            {
                return CheckBrowserSeparatly(ChromeUserAgentConstant, "Chrome", GetBrowserVersionInTheMiddle);
            }

            var supportableBrowser = _browserCompatibilityConfiguration.Browsers.FirstOrDefault(b => String.Equals(b.Name, currentBrowser.Browser, StringComparison.OrdinalIgnoreCase));
            return CheckCompatibility(currentBrowser, supportableBrowser);
        }

        private bool CheckCompatibility(HttpBrowserCapabilitiesBase currentBrowser, Browser supportableBrowser)
        {
            if (supportableBrowser == null)
            {
                return false;
            }
            return CheckVersions(new Version(currentBrowser.Version), new Version(supportableBrowser.Version));
        }

        private bool CheckVersions(Version currentBrowserVersion, Version supportableBrowserVersion)
        {
            return currentBrowserVersion >= supportableBrowserVersion;
        }

        private string GetBrowserVersion(string userAgent, string browserConstant)
        {
            var indexOf = userAgent.IndexOf(browserConstant, StringComparison.OrdinalIgnoreCase) + browserConstant.Length;
            var version = userAgent.Substring(indexOf, userAgent.Length - indexOf);
            return version;
        }

        private string GetBrowserVersionInTheMiddle(string userAgent, string browserConstant)
        {
            var indexOf = userAgent.IndexOf(browserConstant, StringComparison.OrdinalIgnoreCase) + ChromeUserAgentConstant.Length;
            var chromeVersion = string.Empty;
            for (int i = indexOf; i < userAgent.Length; i++)
            {
                if (userAgent[i] == ' ')
                {
                    break;
                }

                chromeVersion = chromeVersion + userAgent[i];
            }

            return chromeVersion;
        }

        private bool IsBrowser(string browserConstant)
        {
            var isBrowser = HttpContext.Request.UserAgent != null && HttpContext.Request.UserAgent.Contains(browserConstant);
            return isBrowser;
        }

        private bool CheckBrowserSeparatly(string browserConstant, string browserName, Func<string, string, string> getBrowserVersion)
        {
            var supportableBrowser = _browserCompatibilityConfiguration.Browsers.FirstOrDefault(b => String.Equals(b.Name, browserName, StringComparison.OrdinalIgnoreCase));
            if (supportableBrowser == null)
            {
                return false;
            }
            var browserVersion = getBrowserVersion(HttpContext.Request.UserAgent, browserConstant);
            return CheckVersions(new Version(browserVersion), new Version(supportableBrowser.Version));

        }
    }
}
