using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Models;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Features.Permissions.TypeProviders;
using Uintra20.Infrastructure.ApplicationSettings;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Services;
using Umbraco.Web.WebApi;

namespace Uintra20.Features.Permissions.Controllers
{
    public class PermissionsController : UmbracoAuthorizedApiController
    {
        private readonly IMemberGroupService _memberGroupService;
        private readonly IPermissionResourceTypeProvider _resourceTypeProvider;
        private readonly IPermissionActionTypeProvider _actionTypeProvider;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IIntranetMemberGroupProvider _intranetMemberGroupProvider;
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;
        private readonly IPermissionsService _permissionsService;
        private readonly IApplicationSettings _applicationSettings;

        public PermissionsController(
            IMemberGroupService memberGroupService,
            IPermissionResourceTypeProvider resourceTypeProvider,
            IPermissionActionTypeProvider actionTypeProvider,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionsService permissionsService,
            IIntranetMemberGroupService intranetMemberGroupService, 
            IApplicationSettings applicationSettings)
        {
            _memberGroupService = memberGroupService;
            _resourceTypeProvider = resourceTypeProvider;
            _actionTypeProvider = actionTypeProvider;
            _intranetMemberService = intranetMemberService;
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _permissionsService = permissionsService;
            _intranetMemberGroupService = intranetMemberGroupService;
            _applicationSettings = applicationSettings;
        }
        
        [HttpGet]
        public async Task<GroupPermissionsViewModel> Get(int memberGroupId)
        {
            var isSuperUser = IsSuperUser();
            var memberGroup = _intranetMemberGroupProvider[memberGroupId];
            var permissions = (await _permissionsService.GetForGroupAsync(memberGroup))
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

        //TODO Refactor this method
        [HttpPost]
        public Task<GroupPermissionsViewModel> Create(MemberGroupCreateModel model)
        {
            var id = _intranetMemberGroupService.Create(model.Name);

            return Get(id);
        }

        [HttpPost]
        public async Task<GroupPermissionsViewModel> Save(PermissionUpdateViewModel update)
        {
            var settingIdentity = new PermissionSettingIdentity(
                _actionTypeProvider[update.ActionId],
                _resourceTypeProvider[update.ResourceTypeId]);
            var settingValue = new PermissionSettingValues(update.Allowed, update.Enabled);
            var targetGroup = _intranetMemberGroupProvider[update.IntranetMemberGroupId];
            //var targetGroup = _intranetMemberGroupService.GetAll().First(x => x.Id == update.IntranetMemberGroupId);

            var mappedUpdate = new PermissionUpdateModel(targetGroup, settingValue, settingIdentity);
            await _permissionsService.SaveAsync(mappedUpdate);

            return await Get(update.IntranetMemberGroupId);
        }

        //TODO Implement correct logic
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int groupId)
        {
            var group = _memberGroupService.GetById(groupId);

            await _permissionsService.DeletePermissionsForMemberGroupAsync(groupId);

            _intranetMemberGroupService.Delete(groupId);

            return Ok();
        }

        private bool IsSuperUser()
        {
            var superUsers = _applicationSettings.UintraSuperUsers;

            var member = _intranetMemberService.GetCurrentMember();

            var isSuperUser = superUsers.Contains(member.Email, StringComparison.InvariantCultureIgnoreCase);

            return isSuperUser;
        }
    }
}