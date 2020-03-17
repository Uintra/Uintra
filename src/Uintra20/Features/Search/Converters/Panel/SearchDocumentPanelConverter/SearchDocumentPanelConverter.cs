using System;
using UBaseline.Shared.Node;

namespace Uintra20.Features.Search.Converters.Panel.SearchDocumentPanelConverter
{
    public abstract class SearchDocumentPanelConverter<TPanel, TSearchDocumentPanel> : ISearchDocumentPanelConverter<TPanel, TSearchDocumentPanel>
        where TPanel : class, INodeViewModel
        where TSearchDocumentPanel : SearchablePanel
    {
        public virtual Type Type { get; } = typeof(TPanel);

        public abstract TSearchDocumentPanel OnConvert(TPanel panel);

        public TSearchDocumentPanel Convert(INodeViewModel data)
        {
            var node = data as TPanel;
            return OnConvert(node);
        }
    }
}