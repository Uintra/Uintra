using UBaseline.Shared.MailchimpPanel;
using Uintra20.Features.Search.Converters.Panel.SearchDocumentPanelConverter;
using Umbraco.Core;

namespace Uintra20.Features.Search.Converters.Panel
{
    public class MailchimpPanelSearchConverter : SearchDocumentPanelConverter<MailchimpPanelViewModel, SearchablePanel>
    {
        public override SearchablePanel OnConvert(MailchimpPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description?.Value?.StripHtml()
            };
        }
    }
}