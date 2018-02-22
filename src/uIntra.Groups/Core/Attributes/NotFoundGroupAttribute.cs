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

            var umbracoHelper = DependencyResolver.Current.GetService<UmbracoHelper>();
            var aliasProvider = DependencyResolver.Current.GetService<IDocumentTypeAliasProvider>();
            var errorPage = umbracoHelper.ContentSingleAtXPath(XPathHelper.GetXpath(aliasProvider.GetHomePage(), aliasProvider.GetErrorPage()));

            if (!groupId.HasValue)
            {
                filterContext.Controller.ControllerContext.HttpContext.Server.TransferRequest(errorPage.Url);
                filterContext.Result = new HttpNotFoundResult();
                return;
            }

            var groupService = DependencyResolver.Current.GetService<IGroupService>();

            var group = groupService.Get(groupId.Value);

            if (group == null || group.IsHidden)
            {                                
                if (errorPage != null)
                {
                    filterContext.Controller.ControllerContext.HttpContext.Server.TransferRequest(errorPage.Url);
                }

                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}