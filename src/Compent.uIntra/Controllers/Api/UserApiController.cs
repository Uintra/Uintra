using uIntra.Users;
using uIntra.Users.Web;
using Umbraco.Core.Services;

namespace Compent.uIntra.Controllers.Api
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