using Uintra.Core.User;
using Uintra.Groups;
using Uintra.Groups.Web;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.Uintra.Controllers
{
    public class GroupDocumentsController : GroupDocumentsControllerBase
    {
        public GroupDocumentsController(
            IGroupDocumentsService groupDocumentsService,
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
