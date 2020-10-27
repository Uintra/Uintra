using UBaseline.Shared.MailchimpPanel;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;
using Umbraco.Core;

namespace Uintra.Features.Search.Converters.Panel
{
    public class MailchimpPanelSearchConverter : SearchDocumentPanelConverter<MailchimpPanelViewModel>
    {
        protected override SearchablePanel OnConvert(MailchimpPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}