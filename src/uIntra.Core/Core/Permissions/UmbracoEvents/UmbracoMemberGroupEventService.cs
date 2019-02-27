using Uintra.Core.Caching;
using Uintra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.Permissions.UmbracoEvents
{
    public class UmbracoMemberGroupEventService : IUmbracoMemberGroupDeletingEventService
    {
        private readonly IPermissionsService _permissionsService;
        private readonly ICacheService _cacheService;

        public UmbracoMemberGroupEventService(IPermissionsService permissionsService,
            ICacheService cacheService)
        {
            _permissionsService = permissionsService;
            _cacheService = cacheService;
        }

        public void ProcessMemberGroupDeleting(IMemberGroupService sender, DeleteEventArgs<IMemberGroup> e)
        {
            foreach (var group in e.DeletedEntities)
            {
                _permissionsService.DeletePermissionsForMemberGroup(group.Id);
            }
        }
    }
}
