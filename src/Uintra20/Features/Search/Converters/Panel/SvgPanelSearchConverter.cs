﻿using UBaseline.Shared.SvgPanel;
 using Uintra20.Core.Search.Converters.SearchDocumentPanelConverter;
 using Uintra20.Core.Search.Entities;

 namespace Uintra20.Features.Search.Converters.Panel
{
    public class SvgPanelSearchConverter : SearchDocumentPanelConverter<SvgPanelViewModel, SearchablePanel>
    {
        public override SearchablePanel OnConvert(SvgPanelViewModel panel)
        {
            return new SearchablePanel
            {
                Title = panel.Title,
                Content = panel.Description
            };
        }
    }
}