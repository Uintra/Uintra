﻿using UBaseline.Shared.QuotePanel;
 using Uintra20.Core.Search.Converters.SearchDocumentPanelConverter;
 using Uintra20.Core.Search.Entities;

 namespace Uintra20.Features.Search.Converters.Panel
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