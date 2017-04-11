using System.Web.Mvc;
using uCommunity.Core.Activity;

namespace uCommunity.Core.User.Permissions
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