using Nest;

namespace Uintra20.Core.Search.Entities
{
    public class SearchableDocument : SearchableBase
    {
        public string Data { get; set; }

        public Attachment Attachment { get; set; }
    }
}