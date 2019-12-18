using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using Uintra20.Core.Authentication;
using Uintra20.Core.Authentication.Models;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Infrastructure.Providers;

namespace Uintra20.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IClientTimezoneProvider clientTimezoneProvider;

        public AuthController(
            IAuthenticationService authenticationService,
            IClientTimezoneProvider clientTimezoneProvider)
        {
            this.authenticationService = authenticationService;
            this.clientTimezoneProvider = clientTimezoneProvider;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IHttpActionResult Login(LoginModelBase loginModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.CollectErrors());

            if (!Membership.ValidateUser(loginModel.Login, loginModel.Password))
                return BadRequest("Credentials not valid");
            
            authenticationService.Login(loginModel.Login, loginModel.Password);
            clientTimezoneProvider.SetClientTimezone(loginModel.ClientTimezoneId);

            return Ok();
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IHttpActionResult> Logout()
        {
            await authenticationService.Logout();

            return Ok();
        }
    }
}