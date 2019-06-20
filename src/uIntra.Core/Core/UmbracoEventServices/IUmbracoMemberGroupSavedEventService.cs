using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEventServices
{
    public interface IUmbracoMemberGroupSavedEventService
    {
        void ProcessMemberGroupSaved(IMemberGroupService sender, SaveEventArgs<IMemberGroup> args);
    }
}
