using Compent.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using Uintra20.Models.UmbracoIdentity;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using UmbracoIdentity;

namespace Uintra20.Core.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UmbracoMembersUserManager<UmbracoApplicationMember> _userManager;
        private readonly IRuntimeState _runtime;
        private readonly IGlobalSettings _globalSettings;

        protected IOwinContext OwinContext => HttpContext.Current.GetOwinContext();

        public AuthenticationService(
            IRuntimeState runtime,
            IGlobalSettings globalSettings)
        {
            _userManager = Umbraco.Core.Composing.Current.Factory.GetInstance<UmbracoMembersUserManager<UmbracoApplicationMember>>();
            _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
            _globalSettings = globalSettings ?? throw new ArgumentNullException(nameof(globalSettings));
        }

        public bool Validate(string login, string password)
        {
            return Membership.ValidateUser(login, password);
        }

        public async void Login(string login, string password)
        {
            var member = await _userManager.FindAsync(login, password);

            OwinContext.Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            OwinContext.Authentication.SignIn(new AuthenticationProperties() { IsPersistent = true },
                await member.GenerateUserIdentityAsync(_userManager));
        }

        public bool IsAuthenticatedRequest(IOwinContext context)
        {
            if (context.Request.Path.Value.In(AnonymousRoutes()))
            {
                return true;
            }

            if (IsClientSideRequest(context.Request.Uri))
            {
                return true;
            }

            var authForBackOffice = ShouldAuthForBackOfficeRequest(context);
            if (authForBackOffice)
            {
                return true;
            }

            return context.Authentication.User?.Identity?.IsAuthenticated == true;
        }

        private static bool IsClientSideRequest(Uri url)
        {
            var ext = Path.GetExtension(url.LocalPath);
            if (ext.IsNullOrWhiteSpace()) return false;
            var toInclude = new[] { ".aspx", ".ashx", ".asmx", ".axd", ".svc" };
            return toInclude.Any(ext.InvariantEquals) == false;
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
				"/ubaseline/api/node/getByUrl"
			};
        }

        private bool ShouldAuthForBackOfficeRequest(IOwinContext ctx)
        {
            if (_runtime.Level == RuntimeLevel.Install)
                return false;

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