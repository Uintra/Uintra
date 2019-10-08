using Compent.Shared.Extensions.Bcl;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace Uintra.Core.UmbracoIpAccess
{
    public class UmbracoIpAccessValidator : IUmbracoIpAccessValidator
    {
        private readonly IUmbracoIpAccessConfiguration ipAccessConfiguration;

        private static UmbracoIpAccessValidator instance;

        public UmbracoIpAccessValidator(IUmbracoIpAccessConfiguration ipAccessConfiguration)
        {
            this.ipAccessConfiguration = ipAccessConfiguration;
        }

        public void Validate(HttpContext httpContext, Assembly controllerAssembly)
        {
            if (!IsAccess(httpContext, controllerAssembly))
            {
                var message = $"Umbraco access denied Ip: {httpContext.Request.UserHostAddress}. Domain: {httpContext.Request.Url?.Host}";

                var user = UmbracoContext.Current.Security.CurrentUser;
                if (user != null)
                {
                    message += $" . User: {user.Email}";
                    UmbracoContext.Current.Security.ClearCurrentLogin();
                }

                LogHelper.Warn<string>(message);
                httpContext.Response.StatusCode = ipAccessConfiguration.StatusCode;
                UmbracoContext.Current.Security.ClearCurrentLogin();
                httpContext.Response.End();
            }
        }

        public bool IsAccess(HttpContext httpContext, Assembly controllerAssembly)
        {
            var isUmbracoRequest = IsUmbracoInstallRequest(httpContext.Request.Url) ||
                IsUmbracoRequest(httpContext.Request.Url);

            if (IsUmbracoRouteRequest(httpContext.Request.Url) || !isUmbracoRequest || IsControllerAssembly(controllerAssembly))
                return true;

            var ip = httpContext.Request.UserHostAddress;
            var domain = httpContext.Request.Url.Host;

            if (ipAccessConfiguration.Ips.IsEmpty() && ipAccessConfiguration.NetworkMasks.IsEmpty() && ipAccessConfiguration.Domains.IsEmpty())
                return true;

            return IsValidIp(ip) || ipAccessConfiguration.Domains.Contains(domain);
        }

        private bool IsValidIp(string ip) => IsInRange(ip) || IsInSubnet(ip);

        private bool IsInSubnet(string ip) =>
            ipAccessConfiguration.NetworkMasks.Any(n => IpMaskComparer.IsInSameSubnet(ip, n.Network, n.Mask));

        private bool IsInRange(string ip) =>
            ipAccessConfiguration.Ips.Any(range => IpComparer.IsGreaterOrEqual(ip, range.From)
                                       && IpComparer.IsLessOrEqual(ip, range.To)
            );

        private static bool IsUmbracoInstallRequest(Uri uri) => uri?.AbsolutePath.InvariantStartsWith("/install/") ?? false;

        private static bool IsUmbracoRequest(Uri uri) => uri?.AbsolutePath.InvariantStartsWith("/umbraco") ?? false;

        private static bool IsUmbracoRouteRequest(Uri uri) => 
            (uri?.AbsolutePath.InvariantStartsWith("/umbraco/surface") ?? false) ||
            (uri?.AbsolutePath.InvariantStartsWith("/umbraco/api") ?? false) ||
            (uri?.AbsolutePath.StartsWith("/umbraco/renderMvc", StringComparison.InvariantCultureIgnoreCase) ?? false);

        private static bool IsControllerAssembly(Assembly controllerAssembly) => controllerAssembly.FullName == Assembly.GetAssembly(typeof(UmbracoIpAccessValidator)).FullName;
    }
}