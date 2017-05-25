using System.Web.Mvc;
using uIntra.Core.Activity;

namespace uIntra.Core.User.Permissions.Web
{
    public class ActivityControllerAttribute:ActionFilterAttribute
    {
        public IntranetActivityTypeEnum ActivityType { get; private set; }

        public ActivityControllerAttribute(IntranetActivityTypeEnum type)
        {
            ActivityType = type;
        }
    }
}