using UBaseline.Shared.ArticleStartPanel;
using Uintra20.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra20.Core.Search.Entities;
using Umbraco.Core;

namespace Uintra20.Features.Search.Converters.Panel
{
    public class ArticleStartPanelSearchConverter : SearchDocumentPanelConverter<ArticleStartPanelViewModel>
    {
        protected override SearchablePanel OnConvert(ArticleStartPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}