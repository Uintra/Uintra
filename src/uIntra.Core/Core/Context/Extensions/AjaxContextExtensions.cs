using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Uintra.Core.Context.Extensions;
using Uintra.Core.Extensions;

namespace Uintra.Core.Context
{
    public static class AjaxContextExtensions
    {

        public static MvcForm BeginContextedForm(this AjaxHelper ajaxHelper, string actionName, string controllerName, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var form = ajaxHelper.BeginForm(actionName, controllerName, ajaxOptions, htmlAttributes);
            ajaxHelper.ViewContext.Writer.Write($"<input name ='context' type='hidden' value='{ajaxHelper.ViewData["context"].ToJson()}'>");
            return form;
        }

        public static MvcHtmlString ContextedActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, AjaxOptions ajaxOptions)
        {
            var routeValuesWithContext = AppendContext(ajaxHelper, new RouteValueDictionary());
            return ajaxHelper.ActionLink(linkText, actionName, routeValuesWithContext, ajaxOptions);
        }

        public static MvcHtmlString ContextedActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var routeValuesDict = ToRouteValueDictionary(routeValues);
            AppendContext(ajaxHelper, routeValuesDict);

            var htmlAttributesDict = ToRouteValueDictionary(htmlAttributes);

            return ajaxHelper.ActionLink(linkText, actionName, controllerName, routeValuesDict, ajaxOptions, htmlAttributesDict);
        }

        public static RouteValueDictionary AppendContext(AjaxHelper ajaxHelper, RouteValueDictionary routeValues)
        {
            routeValues.Add("context", ajaxHelper.ViewData["context"].ToJson());
            return routeValues;
        }

        public static IEnumerable<PropertyInfo> GetProperties(object instance)
        {
            if (instance is null) return Enumerable.Empty<PropertyInfo>();

            var type = instance.GetType();

            var propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(prop => prop.GetIndexParameters().Length == 0 && prop.GetMethod != null);

            return propertyInfos;
        }


        public static RouteValueDictionary ToRouteValueDictionary(object obj)
        {
            var dict = new RouteValueDictionary();
            var properties = PropertyReflectionHelper.GetProperties(obj);

            foreach (var property in properties)
            {
                dict.Add(property.Name, property.GetValue(obj));
            }
            return dict;
        }
    }
}
