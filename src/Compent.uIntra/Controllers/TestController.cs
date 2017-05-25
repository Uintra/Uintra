using Newtonsoft.Json;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Users;
using Umbraco.Web.WebApi;

namespace Compent.uIntra.Controllers
{
    public class TestController : UmbracoApiController
    {
        private readonly IIntranetUserService<IntranetUser> _intranetUserService;
        private readonly IPermissionsConfiguration _permissionsConfiguration;
        private readonly IPermissionsService _permissionsService;

        public TestController(IIntranetUserService<IntranetUser> intranetUserService,
            IPermissionsConfiguration permissionsConfiguration,
            IPermissionsService permissionsService)
        {
            _intranetUserService = intranetUserService;
            _permissionsConfiguration = permissionsConfiguration;
            _permissionsService = permissionsService;
        }

        public string Get(string key)
        {
            return JsonConvert.SerializeObject(_permissionsService.IsRoleHasPermissions(IntranetRolesEnum.WebMaster, key));
        }
    }
}