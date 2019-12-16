using System.Web.Http;
using System.Web.Security;
using Uintra20.Core.Authentication;
using Uintra20.Core.Authentication.Models;
using Uintra20.Infrastructure.Extensions;

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
		public IHttpActionResult Login(LoginModelBase loginModel)
		{
            if (!ModelState.IsValid) return BadRequest(ModelState.CollectErrors());

            if (!Membership.ValidateUser(loginModel.Login, loginModel.Password)) return BadRequest("Credentials not valid");

            _authenticationService.Login(loginModel.Login, loginModel.Password);

			var result = new AuthResultModelBase
            {
				RedirectUrl = loginModel.ReturnUrl ?? "/"
			};

            return Ok(result);
        }
	}
}