using System;
using System.Web;
using System.Web.Mvc;
using LanguageExt;
using Uintra.Core.Extensions;
using Uintra.Groups.Constants;
using static LanguageExt.Prelude;

namespace Uintra.Groups.Attributes
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

        public override void OnActionExecuting(ActionExecutingContext filterContext) =>
            GetGroupPageId(filterContext)
                .Filter(groupId => IsGroupHidden(filterContext, groupId))
                .IfSome(groupId =>
                {
                    var deactivatedGroupLink = _groupLinkProvider.GetDeactivatedGroupLink(groupId);
                    filterContext.HttpContext.Response.Redirect(deactivatedGroupLink);
                });

        private bool IsGroupHidden(ActionExecutingContext filterContext, Guid groupId)
        {
            var httpGroupToken = $"group_is_hidden_{groupId}";
            var isGroupHidden =
                Optional((bool?) filterContext.HttpContext.Items[httpGroupToken])
                    .Choose(() => Optional(_groupService.Get(groupId)?.IsHidden))
                    .IfNone(false);

            filterContext.HttpContext.Items[httpGroupToken] = isGroupHidden;

            return isGroupHidden;
        }

        private static Option<Guid> GetGroupPageId(ActionExecutingContext filterContext) =>
            filterContext.HttpContext.Request.QueryString
                .Get(GroupConstants.GroupIdQueryParam)
                .Apply(parseGuid);
    }
}