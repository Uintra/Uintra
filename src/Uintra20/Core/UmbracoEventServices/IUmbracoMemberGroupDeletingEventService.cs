using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEventServices
{
    public interface IUmbracoMemberGroupDeletingEventService
    {
        void ProcessMemberGroupDeleting(IMemberGroupService sender, DeleteEventArgs<IMemberGroup> e);
    }
}
