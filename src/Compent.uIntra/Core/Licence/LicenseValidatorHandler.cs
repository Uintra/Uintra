using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using UIntra.License.Validator.Context;
using UIntra.License.Validator.Services;
using Umbraco.Core;

namespace Compent.uIntra.Core.Licence
{
    public class LicenseValidatorHandler : ApplicationEventHandler
    {
        private static IEnumerable<string> StaticFileExtensions => new[] { ".js", ".css", ".png", ".ttf", ".img", ".map", ".jpg", ".jpeg", ".ico" };
        private static IEnumerable<string> DisallowedContentTypes => new[] { "application/json", "application/xml" };
        private const string HandlerRequestRegex = ".*\\.axd.*|.*\\.ashx.*|.*asmx.*|.*\\.svc.*";
        private const string StagingEnvironmentRegex = ".*stage.*|.*staging.*|.*preview.*|.*local.*|.*demo.*|.*uat\\..*|.*developer.*|.*\\.local|test\\..*|dev\\..*";
        private const string LicenceViewName = "licence.html";
        private static string LicenceViewPath => string.Concat("~/", LicenceViewName);

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            UmbracoApplicationBase.ApplicationInit += Init;
        }

        private void Init(object sender, EventArgs eventArgs)
        {
            var app = (HttpApplication)sender;
            app.BeginRequest += App_BeginRequest;

        }

        private void App_BeginRequest(object sender, EventArgs e)
        {
            var licenseContext = DependencyResolver.Current.GetService<ILicenseContext>();
            var currentRequest = HttpContext.Current.Request;

            if (licenseContext.IsValid || currentRequest.Url.PathAndQuery.Contains(LicenceViewName))
            {
                return;
            }            

            //bool isValidationSucceeded = IsValidationSucceeded(IsAllowedRequest, currentRequest, IsIgnoredPath(currentRequest.Url.PathAndQuery, currentRequest.Url.Host));

            //if (!isValidationSucceeded)
            //{
            HttpContext.Current.Response.Redirect(LicenceViewPath);
            //}

        }

        private bool IsIgnoredPath(string path, string host)
        {
            bool isHandlerRequest = Regex.IsMatch(path, HandlerRequestRegex, RegexOptions.IgnoreCase);
            bool isStagingEnvironment = Regex.IsMatch(host, StagingEnvironmentRegex, RegexOptions.IgnoreCase);

            return isHandlerRequest || isStagingEnvironment;
        }

        private static bool IsValidationSucceeded(Func<HttpRequest, bool> isAllowedRequest, HttpRequest request, bool isIgnoredPath)
        {
            return isAllowedRequest(request) || IsLicencePage(request.Url, LicenceViewName) || isIgnoredPath;
        }

        private static bool IsAllowedRequest(HttpRequest request)
        {
            var isContentTypeAllowed = new Lazy<bool>(() => IsContentTypeAllowed(DisallowedContentTypes, request.AcceptTypes, request.ContentType));
            return IsStaticFile(request.PhysicalPath) || IsServiceRequest(isContentTypeAllowed, request);
        }

        private static bool IsServiceRequest(Lazy<bool> isContentTypeAllowed, HttpRequest request)
        {
            var isGetRequest = request.HttpMethod != "GET";
            var isAcceptTypesEmpty = request.AcceptTypes == null || !request.AcceptTypes.Any();

            return isGetRequest || !isAcceptTypesEmpty && isContentTypeAllowed.Value;
        }

        private static bool IsContentTypeAllowed(IEnumerable<string> disallowedContentTypes, IEnumerable<string> acceptTypes, string contentType)
        {
            return disallowedContentTypes.Any(x => acceptTypes.Contains(x) || contentType.Contains(x));
        }

        private static bool IsStaticFile(string physicalPath)
        {
            return !string.IsNullOrEmpty(physicalPath) && StaticFileExtensions.Contains(Path.GetExtension(physicalPath));
        }

        private static bool IsLicencePage(Uri url, string licenceViewName)
        {
            return url.ToString().Contains(licenceViewName);
        }
    }
}