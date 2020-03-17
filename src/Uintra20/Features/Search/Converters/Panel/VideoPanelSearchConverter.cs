﻿using UBaseline.Shared.VideoPanel;
 using Uintra20.Core.Search.Converters.SearchDocumentPanelConverter;
 using Uintra20.Core.Search.Entities;

 namespace Uintra20.Features.Search.Converters.Panel
{
    public class VideoPanelSearchConverter : SearchDocumentPanelConverter<VideoPanelViewModel, SearchablePanel>
    {
        public override SearchablePanel OnConvert(VideoPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description
            };
        }
    }
}