using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Compent.Uintra.Core.Licence
{
    public class LicenceRequestHandler : ILicenceRequestHandler

    {
        private IEnumerable<string> StaticFileExtensions => new[] { ".js", ".css", ".png", ".ttf", ".img", ".map", ".jpg", ".jpeg", ".ico" };
        private IEnumerable<string> DisallowedContentTypes => new[] { "application/json", "application/xml" };
        private const string HandlerRequestRegex = ".*\\.axd.*|.*\\.ashx.*|.*asmx.*|.*\\.svc.*";
        private const string StagingEnvironmentRegex = ".*stage.*|.*staging.*|.*preview.*|.*local.*|.*demo.*|.*uat\\..*|.*developer.*|.*\\.local|test\\..*|dev\\..*";
        private const string LicenceViewName = "licence.html";
        private string LicenceViewPath => String.Concat("~/", LicenceViewName);

        private readonly IValidateLicenceService _validateLicenceService;

        public LicenceRequestHandler(IValidateLicenceService validateLicenceService)
        {
            _validateLicenceService = validateLicenceService;
        }

        public void BeginRequestHandler(object obj, EventArgs args)
        {
            HttpRequest currentRequest = HttpContext.Current.Request;

            bool isValidationSucceeded = IsValidationSucceeded(IsAllowedRequest, currentRequest, _validateLicenceService.GetValidationResult());
            if (!isValidationSucceeded)
            {
                HttpContext.Current.Response.Redirect(LicenceViewPath);
            }
        }

        private bool IsValidationSucceeded(Func<HttpRequest, bool> isAllowedRequest, HttpRequest request, bool isLisenceValid)
        {
            return isAllowedRequest(request) || isLisenceValid || IsLicencePage(request.Url, LicenceViewName);
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
            bool isGetRequest = request.HttpMethod != "GET";
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