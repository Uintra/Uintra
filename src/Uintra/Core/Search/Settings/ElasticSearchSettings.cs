using Compent.Shared.Search.Elasticsearch;

namespace Uintra.Core.Search
{
    public class ElasticSearchSettings : IElasticsearchSettings
    {
        public string Host { get; }
        public int Port { get; }
        public string IndexPrefix { get; }
        public string[] HiddenSearchDocumentTypes { get; }
        public int HighlightedFragmentSize { get; }
    }
}