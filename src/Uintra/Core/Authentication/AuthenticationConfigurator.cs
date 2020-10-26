﻿using Owin;

namespace Uintra.Core.Authentication
{
	public class AuthenticationConfigurator
	{
		private readonly IAuthenticationProvider _authentication;

		public AuthenticationConfigurator(IAuthenticationProvider authentication)
		{
			_authentication = authentication;
		}

		public void ConfigureAuthenticationStrategy(IAppBuilder app)
		{
			_authentication.ConfigureAuthenticationProvider(app);
		}
	}
}