using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Permissions.TypeProviders;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Web.WebApi;

namespace Uintra20.Features.Permissions.Controllers
{
    public class PermissionsController : UmbracoAuthorizedApiController
    {
        private readonly IPermissionResourceTypeProvider _resourceTypeProvider;
        private readonly IPermissionActionTypeProvider _actionTypeProvider;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;
        private readonly IPermissionsService _permissionsService;

        public PermissionsController(
            IPermissionResourceTypeProvider resourceTypeProvider,
            IPermissionActionTypeProvider actionTypeProvider,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionsService permissionsService)
        {
            _resourceTypeProvider = resourceTypeProvider;
            _actionTypeProvider = actionTypeProvider;
            _intranetMemberService = intranetMemberService;
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _permissionsService = permissionsService;
        }

        [HttpGet]
        public GroupPermissionsViewModel Get(int memberGroupId)
        {
            var isSuperUser = _intranetMemberService.IsCurrentMemberSuperUser;
            var memberGroup = _intranetMemberGroupProvider[memberGroupId];

            var permissions = _permissionsService
                .GetForGroup(memberGroup)
                .Map<IEnumerable<PermissionViewModel>>()
                .OrderBy(i => i.ResourceTypeId);

            var model = new GroupPermissionsViewModel
            {
                IsSuperUser = isSuperUser,
                Permissions = permissions,
                MemberGroup = memberGroup.Map<MemberGroupViewModel>()
            };

            return model;
        }

        [HttpPost]
        public GroupPermissionsViewModel Save(PermissionUpdateViewModel update)
        {
            var settingIdentity = new PermissionSettingIdentity(
                _actionTypeProvider[update.ActionId],
                _resourceTypeProvider[update.ResourceTypeId]);
            var settingValue = new PermissionSettingValues(update.Allowed, update.Enabled);
            var targetGroup = _intranetMemberGroupProvider[update.IntranetMemberGroupId];

            var mappedUpdate = new PermissionUpdateModel(targetGroup, settingValue, settingIdentity);
            _permissionsService.Save(mappedUpdate);

            return Get(update.IntranetMemberGroupId);
        }
    }
}