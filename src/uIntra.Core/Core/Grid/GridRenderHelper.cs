using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Newtonsoft.Json.Linq;
using Uintra.Core.Helpers;
using HtmlHelper = System.Web.Mvc.HtmlHelper;

namespace Uintra.Core.Grid
{
    public static class GridRenderHelper
    {
        public static MvcHtmlString RenderControl(HtmlHelper htmlHelper, dynamic control)
        {
            try
            {
                var config = control.editor?.config;
                if (config == null || config.action == null || config.controller == null)
                {
                    string editor = EditorPath(control);
                    return htmlHelper.Partial(editor, (object)control);
                }

                string action = config.action;
                string controller = config.controller;

                if (config.includeUmbracoValues != null && (bool)config.includeUmbracoValues)
                {
                    var controllerParams = new RouteValueDictionary(ParseUmbracoValues(control));
                    return htmlHelper.Action(action, controller, controllerParams);
                }

                if (config.includeParams != null && (bool)config.includeParams)
                {
                    var controllerParams = new RouteValueDictionary(ParseParams(config));
                    return htmlHelper.Action(action, controller, controllerParams);
                }

                return htmlHelper.Action(action, controller);
            }
            catch (Exception ex)
            {
                if (HttpContext.Current.IsDebuggingEnabled)
                {
                    return MvcHtmlString.Create($"<pre>{ex}</pre>");
                }
                
                TransferRequestHelper.ToErrorPage();
            }

            return MvcHtmlString.Empty;
        }

        public static bool IsEmptySection(dynamic section)
        {
            return section.rows.Count == 0 || section.rows[0].areas[0].controls.Count == 0;
        }

        public static MvcHtmlString RenderElementAttributes(dynamic contentItem)
        {
            var attrs = new List<string>();
            JObject cfg = contentItem.config;

            if (cfg != null)
            {
                attrs.AddRange(cfg.Properties().Select(p => p.Name + "=\"" + p.Value + "\""));
            }

            JObject style = contentItem.styles;
            if (style != null)
            {
                var cssVals = style.Properties().Select(p => p.Name + ":" + p.Value + ";").ToList();
                if (cssVals.Any())
                    attrs.Add("style=\"" + string.Join(" ", cssVals) + "\"");
            }

            return new MvcHtmlString(string.Join(" ", attrs));
        }

        private static string EditorPath(dynamic control)
        {
            var render = control.editor.render.ToString();
            var view = control.editor.view.ToString();

            var chosen = string.IsNullOrEmpty(render) ? view : render;
            var path = chosen.ToString().ToLower().Replace(".html", ".cshtml");
            return path.Contains("/") ? path : "grid/editors/" + path;
        }

        private static IDictionary<string, object> ParseUmbracoValues(dynamic control)
        {
            return ParseControl(control, "value");
        }

        private static IDictionary<string, object> ParseParams(dynamic control)
        {
            return ParseControl(control, "params");
        }

        private static IDictionary<string, object> ParseControl(dynamic control, string key)
        {
            var result = ((JToken)control)[key].ToObject<Dictionary<string, object>>();
            return result ?? new Dictionary<string, object>();
        }
    }
}
