using System.Collections.Generic;
using System.Linq;
using UBaseline.Core.PanelContainer;
using UBaseline.Shared.PanelContainer;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;
using Uintra.Features.Search.Web;
using Umbraco.Core;

namespace Uintra.Core.Search.Providers
{
    public class SearchContentPanelConverterProvider : ISearchContentPanelConverterProvider
    {
        private readonly IEnumerable<ISearchDocumentPanelConverter> _converters;

        private readonly IPanelContainerBuilder _panelContainerBuilder;

        public SearchContentPanelConverterProvider(
            IEnumerable<ISearchDocumentPanelConverter> converters,
            IPanelContainerBuilder panelContainerBuilder
        )
        {
            this._converters = converters;

            _panelContainerBuilder = panelContainerBuilder;
        }

        public virtual IEnumerable<SearchablePanel> Convert(IPanelsComposition model)
        {
            var panelViewModels = model.Panels.Value.Panels.Select(pm => _panelContainerBuilder.MapNodeViewModel(pm));
            var panelsToSearch = panelViewModels.Select(pvm =>
            {
                var converter = _converters.FirstOrDefault(cvrt => cvrt.Type == pvm.GetType());
                var psdm = converter?.Convert(pvm);
                return psdm;
            }).WhereNotNull();

            return panelsToSearch;
        }
    }
}