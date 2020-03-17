using UBaseline.Shared.ArticleContinuedPanel;
using Uintra20.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra20.Core.Search.Entities;
using Umbraco.Core;

namespace Uintra20.Features.Search.Converters.Panel
{
    public class ArticleContinuedPanelSearchConverter : SearchDocumentPanelConverter<ArticleContinuedPanelViewModel, SearchablePanel>
    {
        public override SearchablePanel OnConvert(ArticleContinuedPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}