using Uintra20.Core.Member;
using Uintra20.Core.Member.Controllers;

namespace Uintra20.Controllers
{
	public class MemberApiController:MemberApiControllerBase
	{
		public MemberApiController(ICacheableIntranetMemberService cacheableIntranetMemberService) : base(cacheableIntranetMemberService)
		{
		}
	}
}