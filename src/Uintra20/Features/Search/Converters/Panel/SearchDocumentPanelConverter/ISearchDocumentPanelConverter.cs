using System;
using UBaseline.Shared.Node;

namespace Uintra20.Features.Search.Converters.Panel.SearchDocumentPanelConverter
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