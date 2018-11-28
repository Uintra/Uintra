using System;
using System.Web.Mvc;
using Uintra.Core;
using Umbraco.Web;

namespace Uintra.Groups.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NotFoundGroupAttribute : ActionFilterAttribute
    {
        private const string GroupIdParameterName = "groupId";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guid? groupId = null;
            if (filterContext.ActionParameters.TryGetValue(GroupIdParameterName, out var obj))
            {
                groupId = obj as Guid?;
            }

            if (!groupId.HasValue)
            {
                TransferRequestToErrorPage(filterContext);
                return;
            }

            var groupService = DependencyResolver.Current.GetService<IGroupService>();
            var group = groupService.Get(groupId.Value);

            if (group == null || group.IsHidden)
            {
                TransferRequestToErrorPage(filterContext);
            }
        }

        private static void TransferRequestToErrorPage(ActionExecutingContext filterContext)
        {
            var umbracoHelper = DependencyResolver.Current.GetService<UmbracoHelper>();
            var aliasProvider = DependencyResolver.Current.GetService<IDocumentTypeAliasProvider>();

            var errorPage = umbracoHelper.TypedContentSingleAtXPath(XPathHelper.GetXpath(aliasProvider.GetHomePage(), aliasProvider.GetErrorPage()));

            if (errorPage != null)
            {
                filterContext.Controller.ControllerContext.HttpContext.Server.TransferRequest(errorPage.Url);
                return;
            }

            filterContext.Result = new HttpNotFoundResult();
        }
    }
}