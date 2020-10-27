using Umbraco.Core.Events;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEvents.Services.Contracts
{
    public interface IUmbracoMemberRemovedRolesEventService
    {
        void MemberRemovedRolesHandler(IMemberService sender, RolesEventArgs e);
    }
}
