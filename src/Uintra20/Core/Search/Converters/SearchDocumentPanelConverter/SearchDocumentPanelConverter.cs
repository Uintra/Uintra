using System;
using UBaseline.Shared.Node;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Core.Search.Converters.SearchDocumentPanelConverter
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