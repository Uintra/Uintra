using System.Linq;
using uIntra.Panels.Core.Models;
using uIntra.Panels.Core.Models.Table;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Panels.Core.ModelBuilders
{
    public class TablePanelModelBuilder : ITablePanelModelBuilder
    {
        
        private readonly ITablePanelColorsModelBuilder _panelColorsModelBuilder;

        public TablePanelModelBuilder(
        
            ITablePanelColorsModelBuilder panelColorsModelBuilder
            )
        {
        
            _panelColorsModelBuilder = panelColorsModelBuilder;
        }

        private const string contentTypeAlias = "tablePanel";
        public virtual string ContentTypeAlias => contentTypeAlias;

        public virtual TablePanelModel Get(IPublishedContent publishedContent)
        {
            var result = new TablePanelModel
            {
                Title = publishedContent.GetPropertyValue<string>(GetTitleAlias()),
                
                BackgroundBehavior = GetBehaviour(publishedContent),
                PanelColors = _panelColorsModelBuilder.Get(publishedContent)
            };

            return result;
        }

        public virtual string GetTitleAlias()
        {
            return nameof(TablePanelModel.Title).ToAlias();
        }

        public virtual string GetTableAlias()
        {
            return nameof(TablePanelModel.Table).ToAlias();
        }

        public virtual PanelBehaviorModel GetBehaviour(IPublishedContent publishedContent)
        {
            if (!IsContains(publishedContent))
            {
                return null;
            }

            var result = new PanelBehaviorModel
            {
                LayoutBehavior = publishedContent.GetPropertyValue<string>(GetLayoutBehaviorAlias())
            };

            return result;
        }

        protected virtual bool IsContains(IPublishedContent publishedContent)
        {
            var result = publishedContent.ContentType.PropertyTypes.Any(i => i.PropertyTypeAlias.Equals(GetLayoutBehaviorAlias()));
            return result;
        }

        public virtual string GetLayoutBehaviorAlias()
        {
            return nameof(PanelBehaviorModel.LayoutBehavior).ToAlias();
        }
    }

    public static class UmbracoStringExtensions
    {
        public static string ToAlias(this string name)
        {
            return name.ToFirstLowerInvariant();
        }

        public static string ToName(this string alias)
        {
            return alias.ToFirstUpperInvariant();
        }
    }

    public class PanelBehaviorModel
    {
        public string LayoutBehavior { get; set; }
    }
}