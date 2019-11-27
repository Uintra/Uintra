using Umbraco.Core.Events;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEventServices
{
    public interface IUmbracoMemberRemovedRolesEventService
    {
        void ProcessMemberRemovedRoles(IMemberService sender, RolesEventArgs e);
    }
}
