using uCommunity.Core.User;
using uCommunity.Navigation.Core;
using uCommunity.Navigation.Web;
using uCommunity.Users.Core;
using Umbraco.Web;

namespace Compent.uCommunity.Controllers
{
    public class MyLinksController : MyLinksControllerBase
    {
        public MyLinksController(UmbracoHelper umbracoHelper, 
            IMyLinksModelBuilder myLinksModelBuilder, 
            IMyLinksService myLinksService, 
            IIntranetUserService<IntranetUser> intranetUserService) 
            : base(umbracoHelper, myLinksModelBuilder, myLinksService, intranetUserService)
        {
        }
    }
}