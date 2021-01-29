using UBaseline.Shared.ArticleContinuedPanel;
using UBaseline.Shared.ContactPanel;
using UBaseline.Shared.TextPanel;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;
using Uintra.Features.UserList.Models;
using Umbraco.Core;

namespace Uintra.Features.Search.Converters.Panel
{
    public class ContactPanelSearchConverter : SearchDocumentPanelConverter<ContactPanelViewModel>
    {
        protected override SearchablePanel OnConvert(ContactPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Content = panel.Title?.Value?.StripHtml()
            };
        }
    }
}