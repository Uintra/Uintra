using Uintra20.Core.Member.Helpers;
using Uintra20.Core.User.Web;
using Umbraco.Core.Services;

namespace Uintra20.Core.Member.Web
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