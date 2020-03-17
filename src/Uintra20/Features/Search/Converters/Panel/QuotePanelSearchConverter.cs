﻿using UBaseline.Shared.QuotePanel;
 using Uintra20.Features.Search.Converters.Panel.SearchDocumentPanelConverter;

 namespace Uintra20.Features.Search.Converters.Panel
{
    public class QuotePanelSearchConverter : SearchDocumentPanelConverter<QuotePanelViewModel, SearchablePanel>
    {
        public override SearchablePanel OnConvert(QuotePanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Quote,
                Content = panel.Description
            };
        }
    }
}