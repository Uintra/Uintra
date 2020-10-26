using System.Collections.Generic;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Features.Search.Web
{
    public interface ISearchContentPanelConverterProvider
    {
        IEnumerable<SearchablePanel> Convert(IPanelsComposition model);
    }
}