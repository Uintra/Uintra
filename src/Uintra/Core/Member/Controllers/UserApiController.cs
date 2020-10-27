using Uintra.Core.Member.Helpers;
using Uintra.Core.User.Web;
using Umbraco.Core.Services;

namespace Uintra.Core.Member.Controllers
{
    public class UserApiController : UserApiControllerBase
    {
        public UserApiController(
            IUserService userService,
            IMemberService memberService,
            IMemberServiceHelper memberServiceHelper):base(userService, memberService, memberServiceHelper)
        {
        }
    }
}