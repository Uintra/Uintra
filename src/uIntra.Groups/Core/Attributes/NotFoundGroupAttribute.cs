using System;
using System.Net;
using System.Web.Mvc;
using LanguageExt;
using Uintra.Core;
using Uintra.Core.Extensions;
using Umbraco.Web;
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
                TransferRequestToErrorPage(filterContext);
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

        private static void TransferRequestToErrorPage(ActionExecutingContext filterContext)
        {
            var umbracoHelper = DependencyResolver.Current.GetService<UmbracoHelper>();
            var aliasProvider = DependencyResolver.Current.GetService<IDocumentTypeAliasProvider>();

            var errorPage = umbracoHelper
                .TypedContentSingleAtXPath(XPathHelper.GetXpath(
                    aliasProvider.GetHomePage(),
                    aliasProvider.GetErrorPage()));

            if (errorPage != null)
            {
                filterContext.Controller.ControllerContext.HttpContext.Response.StatusCode = HttpStatusCode.NotFound.GetHashCode();
                filterContext.Controller.ControllerContext.HttpContext.Response.End();
            }
            else
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}