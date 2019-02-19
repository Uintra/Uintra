using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.Permissions.UmbracoEvents
{
    public class UmbracoMemberGroupEventService : IUmbracoMemberGroupDeletingEventService
    {
        private readonly IBasePermissionsService _basePermissionsService;

        public UmbracoMemberGroupEventService(IBasePermissionsService basePermissionsService)
        {
            _basePermissionsService = basePermissionsService;
        }

        public void ProcessMemberGroupDeleting(IMemberGroupService sender, DeleteEventArgs<IMemberGroup> e)
        {
            foreach (var group in e.DeletedEntities)
            {
                _basePermissionsService.Delete(group.Id);
            }
        }
    }
}
