﻿using UBaseline.Shared.VideoPanel;
 using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
 using Uintra.Core.Search.Entities;

 namespace Uintra.Features.Search.Converters.Panel
{
    public class VideoPanelSearchConverter : SearchDocumentPanelConverter<VideoPanelViewModel>
    {
        protected override SearchablePanel OnConvert(VideoPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description
            };
        }
    }
}