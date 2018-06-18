using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uintra.Core.User;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Uintra.Groups.Web
{
    public class GroupDocumentsController : GroupDocumentsControllerBase
    {
        public GroupDocumentsController(IGroupDocumentsService groupDocumentsService,
            IMediaService mediaService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IGroupMemberService groupMemberService,
            IGroupService groupService,
            UmbracoHelper umbracoHelper,
            IGroupMediaService groupMediaService) :
            base(groupDocumentsService, mediaService, intranetUserService, groupMemberService, groupService, umbracoHelper, groupMediaService)
        {

        }
    }
}
