using Compent.uIntra.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;

namespace uIntra.Core.Web
{
    public class ValidateLicenceHandler : ApplicationEventHandler
    {
        private IEnumerable<string> StaticFileExtensions => new[] { ".js", ".css", ".png", ".ttf", ".img", ".map", ".jpg", ".jpeg", ".ico" };
        private IEnumerable<string> DisallowedContentTypes => new[] {"application/json", "application/xml"};
        private const string StagingEnvironmentRegex = ".*\\.axd.*|.*\\.ashx.*|.*asmx.*|.*\\.svc.*";
        private const string HandlerRequestRegex = ".*stage.*|.*staging.*|.*preview.*|.*demo.*|.*uat\\..*|.*developer.*|.*\\.local|test\\..*|dev\\..*";
        private const string LicenceMessage = "<div style='text-align: center; background-color: #dd0a2d; color: #ffffff; font-size: 20px;  width: 100%; position: fixed; top: 0; left: 0;'><span>Please purchase a licence</span></div>";

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //#if RELEASE
                UmbracoApplicationBase.ApplicationInit += Init;
            //#endif
            UmbracoApplicationBase.ApplicationInit += Init;
            base.ApplicationStarted(umbracoApplication, applicationContext);
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
            validateLicenceService.Validate();
            var isLisenceValid = new Lazy<bool>(() => validateLicenceService.Validate());

            bool requestValidationResult = GetRequestValidationResult(currentRequest, isLisenceValid);
            if (requestValidationResult)
            {
                HttpContext.Current.Response.Write(LicenceMessage);
            }
        }

        private bool GetRequestValidationResult(HttpRequest request, Lazy<bool> isLisenceValid)
        {
            return IsRequestLicenceValidationNeeded(request) && !isLisenceValid.Value;
        }

        private bool IsRequestLicenceValidationNeeded(HttpRequest request)
        {
            return IsStaticFile(request.PhysicalPath) || IsServiceRequest(request) || IsIgnoredPath(request.Path, request.Url.Host);
        }

        private bool IsIgnoredPath(string path, string host)
        {
            bool isHandlerRequest = Regex.IsMatch(path, HandlerRequestRegex, RegexOptions.IgnoreCase);
            bool isStagingEnvironment = Regex.IsMatch(host, StagingEnvironmentRegex, RegexOptions.IgnoreCase);

            return isHandlerRequest || isStagingEnvironment;
        }

        private bool IsServiceRequest(HttpRequest request)
        {
            bool isGetRequest = request.Url.ToString().Contains("?") || request.HttpMethod != "GET";
            bool isAcceptTypesEmpty = request.AcceptTypes == null || !request.AcceptTypes.Any();

            return isGetRequest || (!isAcceptTypesEmpty && IsContentTypeAllowed(DisallowedContentTypes, request.AcceptTypes, request.ContentType));
        }

        private bool IsContentTypeAllowed(IEnumerable<string> disallowedContentTypes, IEnumerable<string> acceptTypes, string contentType)
        {
            return disallowedContentTypes.Any(x => !acceptTypes.Contains(x) ? contentType.Contains(x) : true);
        }

        private bool IsStaticFile(string physicalPath)
        {
            return !string.IsNullOrEmpty(physicalPath) && StaticFileExtensions.Contains(Path.GetExtension(physicalPath));
        }
    }
}