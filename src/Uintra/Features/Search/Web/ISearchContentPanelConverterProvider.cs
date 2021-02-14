using System.Collections.Generic;
using UBaseline.Shared.PanelContainer;
using Uintra.Core.Search.Entities;

namespace Uintra.Features.Search.Web
{
    public interface ISearchContentPanelConverterProvider
    {
        IEnumerable<SearchablePanel> Convert(IPanelsComposition model);
    }
}