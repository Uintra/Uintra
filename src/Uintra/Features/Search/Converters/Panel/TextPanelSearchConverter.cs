using UBaseline.Shared.ArticleContinuedPanel;
using UBaseline.Shared.TextPanel;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;
using Umbraco.Core;

namespace Uintra.Features.Search.Converters.Panel
{
    public class TextPanelSearchConverter : SearchDocumentPanelConverter<TextPanelViewModel>
    {
        protected override SearchablePanel OnConvert(TextPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}