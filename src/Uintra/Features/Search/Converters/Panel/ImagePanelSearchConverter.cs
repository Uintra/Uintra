using UBaseline.Shared.QuotePanel;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;
using Uintra.Features.UintraPanels.ImagePanel.Models;
using Umbraco.Core;

namespace Uintra.Features.Search.Converters.Panel
{
    public class ImagePanelSearchConverter : SearchDocumentPanelConverter<ImagePanelViewModel>
    {
        protected override SearchablePanel OnConvert(ImagePanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title?.Value,
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}