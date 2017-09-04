#define DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Compent.uIntra.Core.Licence;
using Umbraco.Core;

namespace Compent.uIntra.Core.Handlers
{
    public sealed class ValidateLicenceHandler : ApplicationEventHandler
    {
        private IEnumerable<string> StaticFileExtensions => new[] { ".js", ".css", ".png", ".ttf", ".img", ".map", ".jpg", ".jpeg", ".ico" };
        private IEnumerable<string> DisallowedContentTypes => new[] { "application/json", "application/xml" };
        private const string HandlerRequestRegex = ".*\\.axd.*|.*\\.ashx.*|.*asmx.*|.*\\.svc.*";
        private const string StagingEnvironmentRegex = ".*stage.*|.*staging.*|.*preview.*|.*local.*|.*demo.*|.*uat\\..*|.*developer.*|.*\\.local|test\\..*|dev\\..*";
        private const string LicenceViewName = "licence.html";
        private string LicenceViewPath => String.Concat("~/", LicenceViewName);

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
              UmbracoApplicationBase.ApplicationInit += Init;
        }

        private void Init(object sender, EventArgs eventArgs)
        {
            var app = (HttpApplication)sender;
            app.BeginRequest += BeginRequestHandler;
        }

        private void BeginRequestHandler(object obj, EventArgs args)
        {
            var validateLicenceService = DependencyResolver.Current.GetService<IValidateLicenceService>();
            HttpRequest currentRequest = HttpContext.Current.Request;
            var isLisenceValid = new Lazy<bool>(() => validateLicenceService.Validate());

            bool isValidationSucceeded = IsValidationSucceeded(IsAllowedRequest, currentRequest, isLisenceValid);
            if (!isValidationSucceeded)
            {
                HttpContext.Current.Response.Redirect(LicenceViewPath);
            }
        }

        private bool IsValidationSucceeded(Func<HttpRequest, bool> isAllowedRequest, HttpRequest request, Lazy<bool> isLisenceValid)
        {
            return isAllowedRequest(request) || isLisenceValid.Value || IsLicencePage(request.Url, LicenceViewName);
        }

        private bool IsAllowedRequest(HttpRequest request)
        {
            var isContentTypeAllowed = new Lazy<bool>(() => IsContentTypeAllowed(DisallowedContentTypes, request.AcceptTypes, request.ContentType));
            return IsStaticFile(request.PhysicalPath) || IsServiceRequest(isContentTypeAllowed, request) || IsIgnoredPath(request.Path, request.Url.Host);
        }

        private bool IsIgnoredPath(string path, string host)
        {
            bool isHandlerRequest = Regex.IsMatch(path, HandlerRequestRegex, RegexOptions.IgnoreCase);
            bool isStagingEnvironment = Regex.IsMatch(host, StagingEnvironmentRegex, RegexOptions.IgnoreCase);

            return isHandlerRequest || isStagingEnvironment;
        }

        private bool IsServiceRequest(Lazy<bool> isContentTypeAllowed, HttpRequest request)
        {
            bool isGetRequest = request.Url.ToString().Contains('?') || request.HttpMethod != "GET";
            bool isAcceptTypesEmpty = request.AcceptTypes == null || !request.AcceptTypes.Any();

            return isGetRequest || !isAcceptTypesEmpty && isContentTypeAllowed.Value;
        }

        private bool IsContentTypeAllowed(IEnumerable<string> disallowedContentTypes, IEnumerable<string> acceptTypes, string contentType)
        {
            return disallowedContentTypes.Any(x => acceptTypes.Contains(x) || contentType.Contains(x));
        }

        private bool IsStaticFile(string physicalPath)
        {
            return !string.IsNullOrEmpty(physicalPath) && StaticFileExtensions.Contains(Path.GetExtension(physicalPath));
        }

        private bool IsLicencePage(Uri url, string licenceViewName)
        {
            return url.ToString().Contains(licenceViewName);
        }
    }
}