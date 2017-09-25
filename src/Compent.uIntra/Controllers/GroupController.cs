using uIntra.Core.Media;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Web;
using Umbraco.Core.Services;

namespace Compent.uIntra.Controllers
{
    public class GroupController : GroupControllerBase
    {
        public GroupController(IGroupService groupService, 
            IGroupMemberService groupMemberService, 
            IMediaHelper mediaHelper,
            IGroupContentHelper groupContentHelper, 
            IUserService userService, 
            IGroupMediaService groupMediaService, 
            IIntranetUserService<IGroupMember> intranetUserService) 
            : base(groupService, groupMemberService, mediaHelper, groupContentHelper, groupMediaService, intranetUserService)
        {
        }
        
    }
}