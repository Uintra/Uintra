using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Dashboard;

namespace Compent.uIntra.Controllers
{
    public class GroupsSectionController : GroupsSectionControllerBase
    {
        public GroupsSectionController(IGroupService groupsService,
            IGroupHelper groupContentHelper,
            IIntranetUserService<IIntranetUser> intranetUserService) :
            base(groupsService, groupContentHelper, intranetUserService)
        {
        }
    }
}