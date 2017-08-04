using uIntra.Core;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
using uIntra.Navigation;
using uIntra.Navigation.MyLinks;
using uIntra.Navigation.Web;
using uIntra.Users;
using Umbraco.Web;

namespace Compent.uIntra.Controllers
{
    public class MyLinksController : MyLinksControllerBase
    {
        public MyLinksController(UmbracoHelper umbracoHelper, 
            IMyLinksModelBuilder myLinksModelBuilder, 
            IMyLinksService myLinksService, 
            IIntranetUserService<IntranetUser> intranetUserService, 
            IDocumentTypeAliasProvider documentTypeAliasProvider, 
            IActivityTypeProvider activityTypeProvider) 
            : base(umbracoHelper, myLinksModelBuilder, myLinksService, intranetUserService, documentTypeAliasProvider, activityTypeProvider)
        {
        }
    }
}