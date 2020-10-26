using UBaseline.Shared.HeroPanel;
using Uintra20.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra20.Core.Search.Entities;
using Umbraco.Core;

namespace Uintra20.Features.Search.Converters.Panel
{
    public class HeroPanelSearchConverter : SearchDocumentPanelConverter<HeroPanelViewModel>
    {
        protected override SearchablePanel OnConvert(HeroPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}