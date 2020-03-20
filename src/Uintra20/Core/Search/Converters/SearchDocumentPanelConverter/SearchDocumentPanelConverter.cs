using System;
using UBaseline.Shared.Node;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Core.Search.Converters.SearchDocumentPanelConverter
{
    public abstract class SearchDocumentPanelConverter<TPanel> : ISearchDocumentPanelConverter
        where TPanel : class, INodeViewModel
    {
        public virtual Type Type { get; } = typeof(TPanel);

        protected abstract SearchablePanel OnConvert(TPanel panel);

        public SearchablePanel Convert(INodeViewModel data)
        {
            var node = data as TPanel;
            return OnConvert(node);
        }
    }
}