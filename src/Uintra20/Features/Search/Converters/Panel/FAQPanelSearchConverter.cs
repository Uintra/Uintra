using UBaseline.Shared.FAQPanel;
using Uintra20.Features.Search.Converters.Panel.SearchDocumentPanelConverter;
using Umbraco.Core;

namespace Uintra20.Features.Search.Converters.Panel
{
    public class FAQPanelSearchConverter : SearchDocumentPanelConverter<FAQPanelViewModel, SearchablePanel>
    {
        public override SearchablePanel OnConvert(FAQPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}