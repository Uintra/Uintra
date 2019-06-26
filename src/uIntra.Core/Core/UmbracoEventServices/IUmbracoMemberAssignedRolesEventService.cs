
using Umbraco.Core.Events;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEventServices
{
    public interface IUmbracoMemberAssignedRolesEventService
    {
        void ProcessMemberAssignedRoles(IMemberService sender, RolesEventArgs e);
    }
}
