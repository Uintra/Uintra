using UBaseline.Shared.SvgPanel;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;

namespace Uintra.Features.Search.Converters.Panel
{
    public class SvgPanelSearchConverter : SearchDocumentPanelConverter<SvgPanelViewModel>
    {
        protected override SearchablePanel OnConvert(SvgPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description
            };
        }
    }
}