using System.Web.Http;
using System.Web.Security;
using Uintra20.Core.Authentication;
using Uintra20.Core.Authentication.Models;

namespace Uintra20.Controllers
{
	[RoutePrefix("api/auth")]
	public class AuthController : ApiController
	{
		private readonly IAuthenticationService _authenticationService;

		public AuthController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}
		[HttpPost]
		[AllowAnonymous]
		[Route("login")]
		public AuthResultModelBase Login(LoginModelBase loginModel)
		{
			if (/*Validate(loginModel) || for front-end*/ loginModel.Login == "1")
			{
				return new AuthResultModelBase()
				{
					Success = false,
					Message = "Login not filled\n\rPassword is empty\n\r"
				};
			}

			if (/*!Membership.ValidateUser(loginModel.Login, loginModel.Password) || for front-end*/ loginModel.Login == "2")
			{
				return new AuthResultModelBase()
				{
					Success = false,
					Message = "Credentials not valid"
				};
			}


			_authenticationService.Login(loginModel.Login, loginModel.Password);


			return new AuthResultModelBase()
			{
				Success = true,
				RedirectUrl = loginModel.ReturnUrl ?? "/"
			};
		}

		private bool Validate(LoginModelBase loginModel)
		{
			return !string.IsNullOrEmpty(loginModel.Login) && !string.IsNullOrEmpty(loginModel.Password);
		}
	}
}