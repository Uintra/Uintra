using Umbraco.Core.Events;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEventServices
{
    public interface IUmbracoMemberAssignedRolesEventService
    {
        void ProcessMemberAssignedRoles(IMemberService sender, RolesEventArgs e);
    }
}
