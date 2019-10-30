using Compent.Uintra.Core.Constants;
using System;
using System.Web.Mvc;
using Uintra.Core.Helpers;
using Uintra.Groups;
using Umbraco.Web;

namespace Compent.Uintra.Core.Users.UserList
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HasValidGroupIdAttribute : ActionFilterAttribute
    {
        private static readonly IGroupService GroupService;
        private const string GroupIdQueryParameterName = "groupId";

        static HasValidGroupIdAttribute()
        {
            GroupService = DependencyResolver.Current.GetService<IGroupService>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentPage = UmbracoContext.Current.PublishedContentRequest.PublishedContent;

            if (!currentPage.DocumentTypeAlias.Equals(DocumentTypeAliasConstants.GroupsMembersPage))
            {
                return;
            }

            var groupId = filterContext.HttpContext.Request.QueryString[GroupIdQueryParameterName];

            if (groupId == null)
            {
                TransferRequestHelper.ToErrorPage(filterContext);

                return;
            }

            if (Guid.TryParse(groupId, out var id))
            {
                var group = GroupService.Get(id);

                if (group == null || group.IsHidden)
                {
                    TransferRequestHelper.ToErrorPage(filterContext);
                }
            }
            else
            {
                TransferRequestHelper.ToErrorPage(filterContext);
            }
        }
    }
}
