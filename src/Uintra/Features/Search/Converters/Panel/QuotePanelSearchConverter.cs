﻿using UBaseline.Shared.QuotePanel;
 using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
 using Uintra.Core.Search.Entities;

 namespace Uintra.Features.Search.Converters.Panel
{
    public class QuotePanelSearchConverter : SearchDocumentPanelConverter<QuotePanelViewModel>
    {
        protected override SearchablePanel OnConvert(QuotePanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Quote,
                Content = panel.Description
            };
        }
    }
}