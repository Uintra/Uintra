using UBaseline.Shared.DocumentLibraryPanel;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;
using Umbraco.Core;

namespace Uintra.Features.Search.Converters.Panel
{
    public class DocumentLibraryPanelSearchConverter : SearchDocumentPanelConverter<DocumentLibraryPanelViewModel>
    {
        protected override SearchablePanel OnConvert(DocumentLibraryPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.RichTextEditor?.Value?.StripHtml()
            };
        }
    }
}