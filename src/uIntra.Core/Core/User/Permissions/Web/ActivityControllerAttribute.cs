using System.Web.Mvc;
using uIntra.Core.Activity;

namespace uIntra.Core.User.Permissions.Web
{
    public class ActivityControllerAttribute:ActionFilterAttribute
    {
        public int ActivityType { get; private set; }

        public ActivityControllerAttribute(int type)
        {
            ActivityType = type;
        }
    }
}