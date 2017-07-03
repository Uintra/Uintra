using uIntra.Core.TypeProviders;

namespace uIntra.Search
{
    public class SearchableBase
    {
        public object Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public IIntranetType Type { get; set; }
    }
}