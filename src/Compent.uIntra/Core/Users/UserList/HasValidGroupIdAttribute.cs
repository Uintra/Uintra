using Compent.Uintra.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uintra.Core;
using Uintra.Groups;
using Umbraco.Web;

namespace Compent.Uintra.Core.Users.UserList
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HasValidGroupIdAttribute : ActionFilterAttribute
    {
        private const string GroupIdQueryParameterName = "groupId";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentPage = UmbracoContext.Current.PublishedContentRequest.PublishedContent;
            if (!currentPage.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GroupsMembersPage))
                return;

            var groupId = filterContext.HttpContext.Request.QueryString[GroupIdQueryParameterName];
            if (groupId == null)
            {
                TransferRequestToErrorPage(filterContext);
                return;
            }
            if (Guid.TryParse(groupId, out var id))
            {
                var groupService = DependencyResolver.Current.GetService<IGroupService>();
                var group = groupService.Get(id);
                if (group == null || group.IsHidden)
                {
                    TransferRequestToErrorPage(filterContext);
                    return;
                }
            }
            else
            {
                TransferRequestToErrorPage(filterContext);
                return;
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
