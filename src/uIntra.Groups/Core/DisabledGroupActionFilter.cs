using System;
using System.Web;
using System.Web.Mvc;
using uIntra.Core.Extentions;
using uIntra.Groups.Constants;

namespace uIntra.Groups
{
    public class DisabledGroupActionFilter : ActionFilterAttribute
    {
        private readonly IGroupLinkProvider _groupLinkProvider;
        private readonly IGroupService _groupService;

        public DisabledGroupActionFilter()
        {
            _groupService = HttpContext.Current.GetService<IGroupService>();
            _groupLinkProvider = HttpContext.Current.GetService<IGroupLinkProvider>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsGroupPage(filterContext, out Guid groupId))
            {
                if (IsGroupHidden(filterContext, groupId))
                {
                    var deactivatedGroupLink = _groupLinkProvider.GetDeactivatedGroupLink(groupId);
                    filterContext.HttpContext.Response.Redirect(deactivatedGroupLink);
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
            var groupIdValue = filterContext.HttpContext.Request.QueryString.Get(GroupConstants.GroupIdQueryParam);
            return Guid.TryParse(groupIdValue, out groupId);
        }
    }
}