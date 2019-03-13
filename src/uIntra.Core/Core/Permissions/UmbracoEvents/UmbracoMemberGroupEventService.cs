using Uintra.Core.Caching;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.UmbracoEventServices;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.Permissions.UmbracoEvents
{
    public class UmbracoMemberGroupEventService : IUmbracoMemberGroupDeletingEventService, IUmbracoMemberGroupSavedEventService
    {
        private readonly IPermissionsService _permissionsService;
        private readonly ICacheService _cacheService;
        private readonly IIntranetMemberGroupService _intranetMemberGroupService;

        public UmbracoMemberGroupEventService(IPermissionsService permissionsService,
            ICacheService cacheService,
            IIntranetMemberGroupService intranetMemberGroupService)
        {
            _permissionsService = permissionsService;
            _cacheService = cacheService;
            _intranetMemberGroupService = intranetMemberGroupService;
        }

        public void ProcessMemberGroupDeleting(IMemberGroupService sender, DeleteEventArgs<IMemberGroup> e)
        {
            foreach (var group in e.DeletedEntities)
            {
                _permissionsService.DeletePermissionsForMemberGroup(group.Id);
            }
            _intranetMemberGroupService.ClearCache();
        }

        public void ProcessMemberGroupSaved(IMemberGroupService sender, SaveEventArgs<IMemberGroup> args)
        {
            _intranetMemberGroupService.ClearCache();
        }
    }
}
