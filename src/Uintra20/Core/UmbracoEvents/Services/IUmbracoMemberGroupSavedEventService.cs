using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEvents.Services
{
    public interface IUmbracoMemberGroupSavedEventService
    {
        void ProcessMemberGroupSaved(IMemberGroupService sender, SaveEventArgs<IMemberGroup> args);
    }
}
