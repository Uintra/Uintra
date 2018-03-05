using Nest;

namespace Uintra.Search
{
    public class SearchableDocument : SearchableBase
    {
        public string Data { get; set; }

        public Attachment Attachment { get; set; }
    }
}