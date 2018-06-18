using System.Collections.Generic;
using System.Linq;
using Umbraco.Web.WebApi;

namespace Uintra.Core.User.Controllers
{
    public class IntranetUserController : UmbracoAuthorizedApiController
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public IntranetUserController(IIntranetUserService<IIntranetUser> intranetUserService)
        {
            _intranetUserService = intranetUserService;
        }

        public IEnumerable<IIntranetUser> GetAll()
        {
            return _intranetUserService.GetAll().OrderBy(user => user.DisplayedName);
        }
    }
}
