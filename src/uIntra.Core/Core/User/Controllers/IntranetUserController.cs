using System.Collections.Generic;
using System.Linq;
using Umbraco.Web.WebApi;

namespace Uintra.Core.User.Controllers
{
    public class IntranetUserController : UmbracoAuthorizedApiController
    {
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        public IntranetUserController(IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _intranetMemberService = intranetMemberService;
        }

        public IEnumerable<IIntranetMember> GetAll()
        {
            return _intranetMemberService.GetAll().OrderBy(user => user.DisplayedName);
        }
    }
}
