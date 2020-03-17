using UBaseline.Shared.HeroPanel;
using Uintra20.Features.Search.Converters.Panel.SearchDocumentPanelConverter;
using Umbraco.Core;

namespace Uintra20.Features.Search.Converters.Panel
{
    public class HeroPanelSearchConverter : SearchDocumentPanelConverter<HeroPanelViewModel, SearchablePanel>
    {
        public override SearchablePanel OnConvert(HeroPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}