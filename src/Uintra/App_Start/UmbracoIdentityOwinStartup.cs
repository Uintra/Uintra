using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uintra;
using Uintra.Core.Authentication;
using Uintra.Core.Hubs;
using Uintra.Models.UmbracoIdentity;
using Umbraco.Core.Services;
using Umbraco.Web;
using UmbracoIdentity;

[assembly: OwinStartup("UmbracoIdentityOwinStartup", typeof(UmbracoIdentityOwinStartup))]
namespace Uintra
{
	public class UmbracoIdentityOwinStartup : UmbracoIdentityOwinStartupBase
	{
		public override void Configuration(IAppBuilder app)
		{
			base.Configuration(app);

			app.Use(AuthenticationHandler);
		}

		protected override void ConfigureMiddleware(IAppBuilder app)
		{
			// Configure OWIN for authentication.
			ConfigureUmbracoAuthentication(app);

			app.ConfigureSignalR();
			app.FinalizeMiddlewareConfiguration();
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

