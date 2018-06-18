using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Dashboard;

namespace Compent.uIntra.Controllers
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