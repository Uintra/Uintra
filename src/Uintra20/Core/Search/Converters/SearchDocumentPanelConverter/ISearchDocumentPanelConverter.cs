using System;
using UBaseline.Shared.Node;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Core.Search.Converters.SearchDocumentPanelConverter
{
    public interface ISearchDocumentPanelConverter<out TSearchDocumentPanel> where TSearchDocumentPanel : SearchablePanel
    {
        Type Type { get; }

        TSearchDocumentPanel Convert(INodeViewModel panel);
    }

    public interface ISearchDocumentPanelConverter<in TPanel, out TSearchDocumentPanel> : ISearchDocumentPanelConverter<TSearchDocumentPanel>
        where TPanel : INodeViewModel
        where TSearchDocumentPanel : SearchablePanel
    {
    }
}