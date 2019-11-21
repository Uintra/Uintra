using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.Shared.Extensions.Bcl;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Uintra20.Core.Authentication
{
	public class CookieAuthenticationProvider : IAuthenticationProvider
	{

		public void ConfigureAuthenticationProvider(IAppBuilder app)
		{
			app.UseCookieAuthentication(options: new CookieAuthenticationOptions
			{
				CookieName = ".UintraAuth",
				LoginPath = new PathString("/login"),
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
			});
			app.Use(AuthenticationHandler);
		}

		private Task AuthenticationHandler(IOwinContext context, Func<Task> continuation)
		{
			if (AllowRequest(context))
			{
				return continuation();
			}
			context.Authentication.Challenge(DefaultAuthenticationTypes.ApplicationCookie);
			return Task.FromResult(0);
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