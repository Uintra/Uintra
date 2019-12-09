using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web;
using Compent.Shared.Extensions.Bcl;
using Microsoft.Owin.Security;
using UmbracoIdentity;
using Uintra20;
using Uintra20.Models.UmbracoIdentity;
using Uintra20.Core;
using Umbraco.Core;


[assembly: OwinStartup("UmbracoIdentityOwinStartup", typeof(UmbracoIdentityOwinStartup))]
namespace Uintra20
{

	/// <summary>
	/// OWIN Startup class for UmbracoIdentity 
	/// </summary>
	public class UmbracoIdentityOwinStartup : UmbracoIdentityOwinStartupBase
	{
		protected override void ConfigureUmbracoUserManager(IAppBuilder app)
		{
			base.ConfigureUmbracoUserManager(app);

			//Single method to configure the Identity user manager for use with Umbraco
			app.ConfigureUserManagerForUmbracoMembers<UmbracoApplicationMember>();

			//Single method to configure the Identity user manager for use with Umbraco
			app.ConfigureRoleManagerForUmbracoMembers<UmbracoApplicationRole>();
		}

		protected override void ConfigureUmbracoAuthentication(IAppBuilder app)
		{
			base.ConfigureUmbracoAuthentication(app);

			// Enable the application to use a cookie to store information for the 
			// signed in user and to use a cookie to temporarily store information 
			// about a user logging in with a third party login provider 
			// Configure the sign in cookie
			var cookieOptions = CreateFrontEndCookieAuthenticationOptions();
			cookieOptions.LoginPath = new PathString("/login");
			cookieOptions.CookieName = ".UintraAuth";
			cookieOptions.AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie;

			var cmanager = Umbraco.Core.Composing.Current.Factory.GetInstance<UintraFrontEndCookieManager>();
			cookieOptions.CookieManager = cmanager;
			cookieOptions.Provider = new CookieAuthenticationProvider
			{
				// Enables the application to validate the security stamp when the user 
				// logs in. This is a security feature which is used when you 
				// change a password or add an external login to your account.  
				//OnValidateIdentity = SecurityStampValidator
				//		.OnValidateIdentity<UmbracoMembersUserManager<UmbracoApplicationMember>, UmbracoApplicationMember, int>(
				//			TimeSpan.FromMinutes(30),
				//			(manager, user) => user.GenerateUserIdentityAsync(manager),
				//			identity => identity.GetUserId<int>())
			};

			app.UseCookieAuthentication(cookieOptions, PipelineStage.Authenticate);

			// Uncomment the following lines to enable logging in with third party login providers
			//app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
			//app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

			// Enables the application to remember the second login verification factor such as phone or email.
			// Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
			// This is similar to the RememberMe option when you log in.
			//app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

			//app.UseMicrosoftAccountAuthentication(
			//  clientId: "",
			//  clientSecret: "");

			//app.UseTwitterAuthentication(
			//  consumerKey: "",
			//  consumerSecret: "");

			//app.UseFacebookAuthentication(
			//  appId: "",
			//  appSecret: "");

			//app.UseGoogleAuthentication(
			//  clientId: "",
			//  clientSecret: "");


		}

		

		private bool AllowRequest(IOwinContext context)
		{
			return IsValidUser(context) || IsAnonymousRequest(context);
		}

		private bool IsValidUser(IOwinContext context)
		{
			return context.Authentication.User?.Identity?.IsAuthenticated == true;
		}

		private bool IsAnonymousRequest(IOwinContext context)
		{
			return context.Request.Path.Value.In(AnonymousRoutes());
		}

		private string[] AnonymousRoutes()
		{
			return new[]
			{
				"/login",
				"/login/",
				"/api/auth/login"
			};
		}
	}
}

