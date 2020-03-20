using UBaseline.Shared.MailchimpPanel;
using Uintra20.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra20.Core.Search.Entities;
using Umbraco.Core;

namespace Uintra20.Features.Search.Converters.Panel
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