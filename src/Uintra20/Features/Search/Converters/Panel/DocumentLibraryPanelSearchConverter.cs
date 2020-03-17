using UBaseline.Shared.DocumentLibraryPanel;
using Uintra20.Features.Search.Converters.Panel.SearchDocumentPanelConverter;
using Umbraco.Core;

namespace Uintra20.Features.Search.Converters.Panel
{
    public class DocumentLibraryPanelSearchConverter : SearchDocumentPanelConverter<DocumentLibraryPanelViewModel, SearchablePanel>
    {
        public override SearchablePanel OnConvert(DocumentLibraryPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.RichTextEditor?.Value?.StripHtml()
            };
        }
    }
}