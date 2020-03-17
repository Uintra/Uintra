using System;
using UBaseline.Shared.Node;
using Uintra20.Core.Search.Entities;

namespace Uintra20.Core.Search.Converters.SearchDocumentPanelConverter
{
    public interface ISearchDocumentPanelConverter
    {
        Type Type { get; }

        SearchablePanel Convert(INodeViewModel panel);
    }

    public interface ISearchDocumentPanelConverter<in TPanel> : ISearchDocumentPanelConverter
        where TPanel : INodeViewModel
    {
    }
}