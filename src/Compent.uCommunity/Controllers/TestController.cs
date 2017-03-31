using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using uCommunity.Core.User;
using uCommunity.Core.User.Permissions;
using Umbraco.Web.WebApi;

namespace Compent.uCommunity.Controllers
{
    public class TestController : UmbracoApiController
    {
        private readonly IIntranetUserService _intranetUserService;
        private readonly IPermissionsConfiguration _permissionsConfiguration;
        private readonly IPermissionsService _permissionsService;

        public TestController(IIntranetUserService intranetUserService,
            IPermissionsConfiguration permissionsConfiguration,
            IPermissionsService permissionsService)
        {
            _intranetUserService = intranetUserService;
            _permissionsConfiguration = permissionsConfiguration;
            _permissionsService = permissionsService;
        }

        public string Get()
        {
            return JsonConvert.SerializeObject(_permissionsService.IsRoleHasPermissions(IntranetRolesEnum.WebMaster, "CanEditNews"));
        }
    }
}