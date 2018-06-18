using Uintra.Core.User;
using Uintra.Groups;
using Uintra.Groups.Dashboard;

namespace Compent.Uintra.Controllers
{
    public class GroupsSectionController : GroupsSectionControllerBase
    {
        public GroupsSectionController(IGroupService groupsService,
            IGroupLinkProvider groupLinkProvider,
            IIntranetUserService<IIntranetUser> intranetUserService) :
            base(groupsService, groupLinkProvider, intranetUserService)
        {
        }
    }
}