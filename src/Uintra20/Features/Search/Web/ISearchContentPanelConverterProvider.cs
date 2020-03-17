using System.Collections.Generic;
using UBaseline.Shared.Node;
using UBaseline.Shared.PanelContainer;

namespace Uintra20.Features.Search.Web
{
    public interface ISearchContentPanelConverterProvider
    {
        IEnumerable<SearchablePanel> Convert(IPanelsComposition model);
    }
}