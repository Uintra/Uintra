using UBaseline.Shared.FAQPanel;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;
using Umbraco.Core;

namespace Uintra.Features.Search.Converters.Panel
{
    public class FAQPanelSearchConverter : SearchDocumentPanelConverter<FAQPanelViewModel>
    {
        protected override SearchablePanel OnConvert(FAQPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}