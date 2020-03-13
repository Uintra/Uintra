using Uintra20.Core.UmbracoEvents.Services.Contracts;
using Uintra20.Features.Permissions.Interfaces;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEvents.Services.Implementations
{
    public class UmbracoMemberGroupEventService :
        IUmbracoMemberGroupDeletingEventService,
        IUmbracoMemberGroupSavedEventService
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;

        public UmbracoMemberGroupEventService(
            IPermissionsService permissionsService,
            IIntranetMemberGroupService intranetMemberGroupService)
        {
            _permissionsService = permissionsService;
            _intranetMemberGroupService = intranetMemberGroupService;
        }

        public void MemberGroupDeleteHandler(
            IMemberGroupService sender,
            DeleteEventArgs<IMemberGroup> e)
        {
            foreach (var group in e.DeletedEntities)
            {
                _permissionsService.DeletePermissionsForMemberGroup(group.Id);
            }
            _intranetMemberGroupService.ClearCache();
        }

        public void MemberGroupSavedHandler(
            IMemberGroupService sender,
            SaveEventArgs<IMemberGroup> args)
        {
            _intranetMemberGroupService.ClearCache();
        }
    }
}