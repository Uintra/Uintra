using System;
using System.Web;
using System.Web.Mvc;
using uIntra.Core.Extentions;
using uIntra.Groups.Extentions;

namespace uIntra.Groups
{
    public class DisabledGroupActionFilter : ActionFilterAttribute
    {
        private readonly IGroupContentHelper _groupContentHelper;
        private readonly IGroupService _groupService;

        public DisabledGroupActionFilter()
        {
            _groupService = HttpContext.Current.GetService<IGroupService>();
            _groupContentHelper = HttpContext.Current.GetService<IGroupContentHelper>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guid groupId;
            if (IsGroupPage(filterContext, out groupId))
            {
                if (IsGroupHidden(filterContext, groupId))
                {
                    var disabledGroupPage = _groupContentHelper.GetDeactivatedGroupPage();

                    filterContext.HttpContext.Response.Redirect(disabledGroupPage.UrlWithGroupId(groupId));
                }
            }
        }

        private bool IsGroupHidden(ActionExecutingContext filterContext, Guid groupId)
        {
            var sessionGroupValue = (bool?)filterContext.HttpContext.Items[$"group_is_hidden_{groupId}"];

            if (sessionGroupValue.HasValue)
            {
                return sessionGroupValue.Value;
            }
            var group = _groupService.Get(groupId);
            filterContext.HttpContext.Items[$"group_is_hidden_{groupId}"] = group.IsHidden;
            return group.IsHidden;
        }

        private bool IsGroupPage(ActionExecutingContext filterContext, out Guid groupId)
        {
            var groupIdValue = filterContext.HttpContext.Request.QueryString.Get("groupId");
            return Guid.TryParse(groupIdValue, out groupId);
        }
    }
}