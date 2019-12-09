using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Uintra20.Models.UmbracoIdentity;
using UmbracoIdentity;
using Umbraco.Core;

namespace Uintra20.Core.Authentication
{
	public class AuthenticationService: IAuthenticationService
	{
		private readonly UmbracoMembersUserManager<UmbracoApplicationMember> _userManager;

		private IAuthenticationManager AuthenticationManager => HttpContext.Current.GetOwinContext().Authentication;
		protected IOwinContext OwinContext
		{
			get { return HttpContext.Current.GetOwinContext(); }
		}

		public AuthenticationService()
		{
			_userManager = Umbraco.Core.Composing.Current.Factory.GetInstance<UmbracoMembersUserManager<UmbracoApplicationMember>>();
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

		private ClaimsIdentity GetIdentity(string userName)
		{
			var claims = new List<Claim> {new Claim(ClaimTypes.NameIdentifier, userName)};
			var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
			return identity;
		}
	}
}