using Uintra.Users;
using Uintra.Users.Web;
using Umbraco.Core.Services;

namespace Compent.Uintra.Controllers.Api
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