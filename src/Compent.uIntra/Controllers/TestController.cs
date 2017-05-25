using Newtonsoft.Json;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions;
using uCommunity.Users.Core;
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