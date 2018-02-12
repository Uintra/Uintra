using Nest;

namespace Uintra.Search
{
    public class SearchableBaseMap<T> : PropertiesDescriptor<T> where T : SearchableBase
    {
        public SearchableBaseMap()
        {
            Text(t => t.Name(n => n.Id));
            Text(t => t.Name(n => n.Title).Analyzer(ElasticHelpers.ReplaceNgram));
            Number(t => t.Name(n => n.Type).Type(NumberType.Integer));
            Text(t => t.Name(n => n.Url));
        }
    }
}