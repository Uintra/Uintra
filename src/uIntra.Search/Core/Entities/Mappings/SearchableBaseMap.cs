using Nest;

namespace uIntra.Search
{
    public class SearchableBaseMap<T> : PropertiesDescriptor<T> where T : SearchableBase
    {
        public SearchableBaseMap()
        {
            Text(t => t.Name(n => n.Id));
            Text(t => t.Name(n => n.Title).Analyzer(ElasticHelpers.ReplaceNgram));
            Text(t => t.Name(n => n.Type));
            Text(t => t.Name(n => n.Url));
        }
    }
}