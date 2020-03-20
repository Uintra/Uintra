namespace Uintra20.Core.Search.Indexers.Diagnostics.Models
{
    public class IndexedModelResult
    {
        public bool Success { get; set; }
        public int IndexedItems { get; set; }
        public string IndexedName { get; set; }
        public string Message { get; set; }
    }
}