using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Uintra20.Core.Authentication
{
	public class AuthenticationService: IAuthenticationService
	{
		private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;

		public bool Validate(string login, string password)
		{
			return Membership.ValidateUser(login, password);
		}

		public void Login(string login, string password)
		{
			var identity = GetIdentity(login);
			AuthenticationManager.SignIn(identity);
		}

		public void Logout()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
		}

		private ClaimsIdentity GetIdentity(string userName)
		{
			var claims = new List<Claim> {new Claim(ClaimTypes.NameIdentifier, userName)};
			var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
			return identity;
		}
	}
}