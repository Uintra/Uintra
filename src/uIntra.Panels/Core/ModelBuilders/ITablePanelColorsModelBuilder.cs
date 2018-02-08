using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Panels.Core.Models;
using uIntra.Panels.Core.Models.Table;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace uIntra.Panels.Core.ModelBuilders
{
    public interface ITablePanelColorsModelBuilder
    {
        string ContentTypeAlias { get; }
        TablePanelColorsModel Get(IPublishedContent publishedContent);
        string GetPanelColorPickerAlias();
    }

    public class TablePanelColorsModelBuilder : ITablePanelColorsModelBuilder
    {
        private const string PanelColorPickerAlias = "color";
        
        private const string contentTypeAlias = "panelColors";
        public virtual string ContentTypeAlias => contentTypeAlias;

        public virtual TablePanelColorsModel Get(IPublishedContent publishedContent)
        {
            if (!IsContains(publishedContent))
            {
                return null;
            }

            var propertyValue = publishedContent.GetPropertyValue<string>(GetPanelColorPickerAlias());

            var result = propertyValue?.Deserialize<TablePanelColorsModel>();
            return result;
        }

        protected virtual bool IsContains(IPublishedContent publishedContent)
        {
            var result = publishedContent.ContentType.PropertyTypes.Any(i => i.PropertyTypeAlias.Equals(GetPanelColorPickerAlias()));
            return result;
        }

        public virtual string GetPanelColorPickerAlias()
        {
            return PanelColorPickerAlias;
        }
    }
}
