using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uintra20;
using Uintra20.Core.Authentication;
using Uintra20.Models.UmbracoIdentity;
using UmbracoIdentity;

[assembly: OwinStartup("UmbracoIdentityOwinStartup", typeof(UmbracoIdentityOwinStartup))]
namespace Uintra20
{
    public class UmbracoIdentityOwinStartup : UmbracoIdentityOwinStartupBase
	{
        public override void Configuration(IAppBuilder app)
        {
            base.Configuration(app);

            app.Use(AuthenticationHandler);
        }

        protected override void ConfigureUmbracoUserManager(IAppBuilder app)
		{
			base.ConfigureUmbracoUserManager(app);

			app.ConfigureUserManagerForUmbracoMembers<UmbracoApplicationMember>();
			app.ConfigureRoleManagerForUmbracoMembers<UmbracoApplicationRole>();
		}

		protected override void ConfigureUmbracoAuthentication(IAppBuilder app)
		{
			base.ConfigureUmbracoAuthentication(app);

			var cookieOptions = CreateFrontEndCookieAuthenticationOptions();
			cookieOptions.LoginPath = new PathString("/login");
			cookieOptions.CookieName = ".UintraAuth";
			cookieOptions.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie;

			app.UseCookieAuthentication(cookieOptions, PipelineStage.Authenticate);
		}

        private Task AuthenticationHandler(IOwinContext context, Func<Task> continuation)
        {
            var authenticationService = DependencyResolver.Current.GetService<IAuthenticationService>();
            if (authenticationService.IsAuthenticatedRequest(context))
            {
                return continuation();
            }

            context.Authentication.Challenge(DefaultAuthenticationTypes.ApplicationCookie);
            return Task.FromResult(0);
        }
    }
}

