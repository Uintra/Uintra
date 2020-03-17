using UBaseline.Shared.NewsPanel;
using Uintra20.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra20.Core.Search.Entities;
using Umbraco.Core;

namespace Uintra20.Features.Search.Converters.Panel
{
    public class NewsPanelSearchConverter : SearchDocumentPanelConverter<NewsPanelViewModel, SearchablePanel>
    {
        public override SearchablePanel OnConvert(NewsPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}