using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.User;
using Uintra.Core.Web;

namespace Compent.Uintra.Controllers
{
    public class ActivityController: ActivityControllerBase
    {
        protected override string ItemHeaderViewPath { get; } = "~/Views/Activity/ItemHeader.cshtml";

        public ActivityController(IIntranetMemberService<IIntranetMember> intranetMemberService,
            IPermissionsService basePermissionsService) 
            : base(intranetMemberService, basePermissionsService)
        {
        }
    }
}