using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEvents.Services.Contracts
{
    public interface IUmbracoMemberGroupDeletingEventService
    {
        void ProcessMemberGroupDeleting(IMemberGroupService sender, DeleteEventArgs<IMemberGroup> e);
    }
}
