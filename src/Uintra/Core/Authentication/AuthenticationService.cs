using Compent.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Models.UmbracoIdentity;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Services;
using UmbracoIdentity;

namespace Uintra.Core.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UmbracoMembersUserManager<UmbracoApplicationMember> _userManager;
        private readonly IRuntimeState _runtime;
        private readonly IGlobalSettings _globalSettings;
        private readonly IMemberService _memberService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        protected IOwinContext OwinContext => HttpContext.Current.GetOwinContext();

        public AuthenticationService(
            IRuntimeState runtime,
            IGlobalSettings globalSettings,
            IMemberService memberService, 
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            //_userManager = Umbraco.Core.Composing.Current.Factory.GetInstance<UmbracoMembersUserManager<UmbracoApplicationMember>>();
            //_userManager = OwinContext.Get<UmbracoMembersUserManager<UmbracoApplicationMember>>();
            _userManager = OwinContext.GetUserManager<UmbracoMembersUserManager<UmbracoApplicationMember>>();
            _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
            _globalSettings = globalSettings ?? throw new ArgumentNullException(nameof(globalSettings));
            _memberService = memberService;
            _intranetMemberService = intranetMemberService;
        }

        public bool Validate(string login, string password)
        {
            return Membership.ValidateUser(login, password);
        }

        public async Task LoginAsync(string login, string password)
        {
            var member = await _userManager.FindAsync(login, password);
            //var identity = await member.GenerateUserIdentityAsync(_userManager);
            var identity = await _userManager.CreateIdentityAsync(member, DefaultAuthenticationTypes.ApplicationCookie);
            var id = _memberService.GetById(member.Id).Key;

            identity.AddClaim(new Claim("UserId", id.ToString()));

            //OwinContext.Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //OwinContext.Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            
            OwinContext.Authentication.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);
        }

        public bool Logout()
        {
            OwinContext.Authentication.SignOut(
                DefaultAuthenticationTypes.ApplicationCookie,
                DefaultAuthenticationTypes.ExternalCookie);

            return true;
        }

        public bool IsAuthenticatedRequest(IOwinContext context)
        {

            if (context.Request.Path.Value.In(AnonymousRoutes()))
            {
                return true;
            }

            if (IsFileRequest(context))
            {
                return true;
            }

            var authForBackOffice = ShouldAuthForBackOfficeRequest(context);
            if (authForBackOffice)
            {
                return true;
            }

            var member = _intranetMemberService.GetByName(OwinContext.Authentication.User.Identities.FirstOrDefault()?.Name);

            if (member == null || member.Inactive)
            {
                Logout();
                return false;
            }


            return context.Authentication.User?.Identity?.IsAuthenticated == true;
        }

        //private static bool IsClientSideRequest(Uri url)
        //{
        //    var ext = Path.GetExtension(url.LocalPath);
        //    if (ext.IsNullOrWhiteSpace()) return false;
        //    var toInclude = new[] { ".aspx", ".ashx", ".asmx", ".axd", ".svc" };
        //    return toInclude.Any(ext.InvariantEquals) == false;
        //}

        private static bool IsFileRequest(IOwinContext context)
        {
            var localPath = context.Request.Uri.LocalPath;
            if (localPath.Contains("media") &&
                !context.Request.Cookies.HasValue(i => i.Key.InvariantEquals(".UintraAuth"))) return false;

            var ext = Path.GetExtension(localPath);

            //TODO, will be replaced by correct authentication flow from uBaseline project.
            return !ext.IsNullOrWhiteSpace();

            if (ext.IsNullOrWhiteSpace()) return false;
            var toInclude = new[] { ".aspx", ".ashx", ".asmx", ".axd", ".svc", ".html", ".css", ".woff2", ".js", ".ttf", ".woff", ".ico", ".svg", ".jpeg", ".png", ".jpg", ".gif",".mp4" };
            return toInclude.Any(ext.InvariantEquals);
        }

        private static bool IsBackOfficeRequest(IOwinRequest request, IGlobalSettings globalSettings)
        {
            return (bool)typeof(UriExtensions).CallStaticMethod("IsBackOfficeRequest", request.Uri, HttpRuntime.AppDomainAppVirtualPath, globalSettings);
        }

        private static bool IsInstallerRequest(IOwinRequest request)
        {
            return (bool)typeof(UriExtensions).CallStaticMethod("IsInstallerRequest", request.Uri);
        }

        private static string[] AnonymousRoutes()
        {
            return new[]
            {
                "/login",
                "/login/",
                "/api/auth/login",
                "/ubaseline/api/node/getByUrl",
                "/ubaseline/api/localization/getAll",
                "/ubaseline/api/search/rebuildIndex",
                "/ubaseline/api/CentralFeedApi/AvailableActivityTypes",
                "/signalr/connect"
            };
        }

        private bool ShouldAuthForBackOfficeRequest(IOwinContext ctx)
        {
            if (_runtime.Level == RuntimeLevel.Install)
                return true;

            var request = ctx.Request;

            if (//check back office
                IsBackOfficeRequest(request, _globalSettings)
                //check installer
                || IsInstallerRequest(request))
            {
                return true;
            }
            return false;
        }
    }
}