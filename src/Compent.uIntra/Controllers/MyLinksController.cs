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
            IIntranetMemberService<IIntranetMember> intranetMemberService, 
            IDocumentTypeAliasProvider documentTypeAliasProvider, 
            IActivityTypeProvider activityTypeProvider) 
            : base(umbracoHelper, myLinksModelBuilder, myLinksService, intranetMemberService, documentTypeAliasProvider, activityTypeProvider)
        {
        }
    }
}