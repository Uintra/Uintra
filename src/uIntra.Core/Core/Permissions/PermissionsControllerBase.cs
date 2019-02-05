using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Permissions.TypeProviders;
using Uintra.Core.TypeProviders;
using Umbraco.Web.Mvc;

namespace Uintra.Core.Permissions
{    
    abstract class PermissionsControllerBase : SurfaceController
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IActivityTypeProvider _activityTypeProvider;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;

        protected PermissionsControllerBase(IPermissionsService permissionsService, IActivityTypeProvider activityTypeProvider, IIntranetMemberGroupProvider intranetMemberGroupProvider)
        {
            _permissionsService = permissionsService;
            _activityTypeProvider = activityTypeProvider;
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
        }

        public IEnumerable<PermissionViewModel> Get(int memberGroupId)
        {
            var allActivityTypes = _activityTypeProvider.All;
            var memberGroup = _intranetMemberGroupProvider[memberGroupId];

            allActivityTypes.Select(at => { _permissionsService.GetForGroup(at.ToInt(), memberGroup); })
            
            

        }
    }
}
