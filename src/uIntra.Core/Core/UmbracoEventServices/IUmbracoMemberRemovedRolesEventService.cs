using Umbraco.Core.Events;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEventServices
{
    public interface IUmbracoMemberRemovedRolesEventService
    {
        void ProcessMemberRemovedRoles(IMemberService sender, RolesEventArgs e);
    }
}
