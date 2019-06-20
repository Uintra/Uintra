﻿using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Uintra.Core.Activity;
using Umbraco.Web;

namespace Uintra.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NotFoundActivityAttribute : ActionFilterAttribute
    {
        private const string ActivityIdParameterName = "id";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Guid? activityId = null;
            if (filterContext.ActionParameters.TryGetValue(ActivityIdParameterName, out var obj))
            {
                activityId = obj as Guid?;
            }

            var activityServices = DependencyResolver.Current.GetServices<IIntranetActivityService<IIntranetActivity>>();
            var activity = activityServices.Select(service => service.Get(activityId.GetValueOrDefault())).FirstOrDefault(a => a != null);

            if (activity == null || activity.IsHidden)
            {
                var umbracoHelper = DependencyResolver.Current.GetService<UmbracoHelper>();
                var aliasProvider = DependencyResolver.Current.GetService<IDocumentTypeAliasProvider>();

                var errorPage = umbracoHelper.ContentSingleAtXPath(XPathHelper.GetXpath(aliasProvider.GetHomePage(), aliasProvider.GetErrorPage()));
                if (errorPage != null)
                {
                    filterContext.Controller.ControllerContext.HttpContext.Response.StatusCode = HttpStatusCode.NotFound.GetHashCode();
                    filterContext.Controller.ControllerContext.HttpContext.Response.End();
                }

                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}