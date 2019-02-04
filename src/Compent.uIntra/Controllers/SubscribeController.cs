using Localization.Umbraco.Attributes;
using Uintra.Core.Activity;
using Uintra.Core.User;
using Uintra.Subscribe;
using Uintra.Subscribe.Web;

namespace Compent.Uintra.Controllers
{
    [ThreadCulture]
    public class SubscribeController : SubscribeControllerBase
    {
        public SubscribeController(
            ISubscribeService subscribeService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IActivitiesServiceFactory activitiesServiceFactory) :
            base(subscribeService, intranetMemberService, activitiesServiceFactory)
        {
        }
    }
}