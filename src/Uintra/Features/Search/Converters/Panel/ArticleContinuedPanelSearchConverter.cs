using UBaseline.Shared.ArticleContinuedPanel;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;
using Umbraco.Core;

namespace Uintra.Features.Search.Converters.Panel
{
    public class ArticleContinuedPanelSearchConverter : SearchDocumentPanelConverter<ArticleContinuedPanelViewModel>
    {
        protected override SearchablePanel OnConvert(ArticleContinuedPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}