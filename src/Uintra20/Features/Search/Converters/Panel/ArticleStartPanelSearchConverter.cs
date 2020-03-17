using UBaseline.Shared.ArticleStartPanel;
using Uintra20.Features.Search.Converters.Panel.SearchDocumentPanelConverter;
using Umbraco.Core;

namespace Uintra20.Features.Search.Converters.Panel
{
    public class ArticleStartPanelSearchConverter : SearchDocumentPanelConverter<ArticleStartPanelViewModel, SearchablePanel>
    {
        public override SearchablePanel OnConvert(ArticleStartPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}