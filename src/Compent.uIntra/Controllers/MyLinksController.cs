using Uintra.Core;
using Uintra.Core.TypeProviders;
using Uintra.Core.User;
using Uintra.Navigation;
using Uintra.Navigation.MyLinks;
using Uintra.Navigation.Web;
using Umbraco.Web;

namespace Compent.Uintra.Controllers
{
    public class MyLinksController : MyLinksControllerBase
    {
        public MyLinksController(UmbracoHelper umbracoHelper, 
            IMyLinksModelBuilder myLinksModelBuilder, 
            IMyLinksService myLinksService, 
            IIntranetUserService<IIntranetUser> intranetUserService, 
            IDocumentTypeAliasProvider documentTypeAliasProvider, 
            IActivityTypeProvider activityTypeProvider) 
            : base(umbracoHelper, myLinksModelBuilder, myLinksService, intranetUserService, documentTypeAliasProvider, activityTypeProvider)
        {
        }
    }
}