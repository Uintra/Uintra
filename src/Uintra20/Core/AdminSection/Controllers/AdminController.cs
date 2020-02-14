using System.Threading.Tasks;
using System.Web.Http;
using Compent.Shared.Extensions.Bcl;
using UBaseline.Core.Controllers;
using Uintra20.Features.Permissions.Migrations;
using Uintra20.Infrastructure.ApplicationSettings;

namespace Uintra20.Core.AdminSection.Controllers
{
    //TODO Create normal controller
    public class AdminController : UBaselineApiController
    {
        private readonly IApplicationSettings _applicationSettings;

        public AdminController(IApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }
        [HttpGet]
        public async Task<IHttpActionResult> SetupDefaultGroupPermissions(string secretApiKey)
        {
            if (!secretApiKey.HasValue() || !_applicationSettings.AdminControllerSecretKey.Equals(secretApiKey))
                return BadRequest();
            

            await new DefaultMemberGroupPermissionsInitializer().SetupDefaultPermissions();
            return Ok();
        }
    }
}