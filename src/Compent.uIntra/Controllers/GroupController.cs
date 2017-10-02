using uIntra.Core.Links;
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
            IGroupHelper groupContentHelper, 
            IUserService userService, 
            IGroupMediaService groupMediaService, 
            IIntranetUserService<IGroupMember> intranetUserService, IProfileLinkProvider profileLinkProvider) 
            : base(groupService, groupMemberService, mediaHelper, groupContentHelper, groupMediaService, intranetUserService, profileLinkProvider)
        {
        }
        
    }
}