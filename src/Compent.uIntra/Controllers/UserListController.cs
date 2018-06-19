using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra.Core.User;
using Uintra.Users.Web;

namespace Compent.Uintra.Controllers
{
    public class UserListController : UserListControllerBase
    {
        public UserListController(IIntranetUserService<IIntranetUser> intranetUserService) 
            :base(intranetUserService)
        {

        }
    }
}