using System;
using System.Web.Mvc;
using LanguageExt;
using Uintra.Core.Extensions;
using Uintra.Core.Helpers;
using static LanguageExt.Prelude;

namespace Uintra.Groups.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NotFoundGroupAttribute : ActionFilterAttribute
    {
        private const string GroupIdParameterName = "groupId";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var groupId = GetGroupIdFromRequest(filterContext);
            var validGroupId = groupId.Filter(IsValidGroupId);

            if (groupId.IsSome && validGroupId.IsNone)
            {
                TransferRequestHelper.ToErrorPage(filterContext);
            }
        }

        private static Option<Guid> GetGroupIdFromActionParameters(ActionExecutingContext filterContext) =>
            filterContext.ActionParameters
                .TryGetValue(GroupIdParameterName)
                .Map(EnumerableExtensions.Cast<Guid>);

        private static Option<Guid> GetGroupIdUrlParameters(ActionExecutingContext filterContext) =>
            Optional(filterContext.HttpContext.Request.QueryString[GroupIdParameterName])
                .Map(Guid.Parse);


        private static Option<Guid> GetGroupIdFromRequest(ActionExecutingContext filterContext) =>
            GetGroupIdFromActionParameters(filterContext)
                .Choose(() => GetGroupIdUrlParameters(filterContext));


        private static bool IsValidGroupId(Guid groupId)
        {
            var requestGroup = Optional(DependencyResolver.Current.GetService<IGroupService>().Get(groupId));
            var notHiddenGroup = requestGroup.Filter(group => !group.IsHidden);
            return notHiddenGroup.IsSome;
        }
    }
}