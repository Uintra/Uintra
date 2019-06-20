using Uintra.Core.User;
using Uintra.Users.Web;

namespace Compent.Uintra.Controllers.Api
{
    public class MemberManagementApiController : MemberManagementApiControllerBase
    {
        public MemberManagementApiController(IIntranetMemberService<IIntranetMember> intranetMemberService) : base(intranetMemberService)
        {
        }
    }
}