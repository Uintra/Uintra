using Umbraco.Core.Events;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEvents.Services.Contracts
{
    public interface IUmbracoMemberAssignedRolesEventService
    {
        void MemberAssignedRolesHandler(IMemberService sender, RolesEventArgs e);
    }
}
