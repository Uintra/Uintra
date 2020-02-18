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
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models;
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

        public PermissionsController(
            IMemberGroupService memberGroupService,
            IPermissionResourceTypeProvider resourceTypeProvider,
            IPermissionActionTypeProvider actionTypeProvider,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IIntranetMemberGroupProvider intranetMemberGroupProvider,
            IPermissionsService permissionsService,
            IIntranetMemberGroupService intranetMemberGroupService)
        {
            _memberGroupService = memberGroupService;
            _resourceTypeProvider = resourceTypeProvider;
            _actionTypeProvider = actionTypeProvider;
            _intranetMemberService = intranetMemberService;
            _intranetMemberGroupProvider = intranetMemberGroupProvider;
            _permissionsService = permissionsService;
            _intranetMemberGroupService = intranetMemberGroupService;
        }
        
        [HttpGet]
        public async Task<GroupPermissionsViewModel> Get(int memberGroupId)
        {
            var isSuperUser = _intranetMemberService.IsCurrentMemberSuperUser;
            //var memberGroup = _intranetMemberGroupProvider[memberGroupId];
            var memberGroup = _intranetMemberGroupService.GetAll().First(x => x.Id == memberGroupId);

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
            //var umbracoMember = new MemberGroup
            //{
            //    Name = model.Name
            //};
            //_memberGroupService.Save(umbracoMember);

            var id = _intranetMemberGroupService.Create(model.Name);
            _intranetMemberGroupService.ClearCache();

            //TODO Set Default permissions

            return Get(id);
        }

        [HttpPost]
        public async Task<GroupPermissionsViewModel> Save(PermissionUpdateViewModel update)
        {
            var settingIdentity = new PermissionSettingIdentity(
                _actionTypeProvider[update.ActionId],
                _resourceTypeProvider[update.ResourceTypeId]);
            var settingValue = new PermissionSettingValues(update.Allowed, update.Enabled);
            //var targetGroup = _intranetMemberGroupProvider[update.IntranetMemberGroupId];
            var targetGroup = _intranetMemberGroupService.GetAll().First(x => x.Id == update.IntranetMemberGroupId);

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

            _memberGroupService.Delete(group);
            _intranetMemberGroupService.ClearCache();

            return Ok();
        }
    }
}