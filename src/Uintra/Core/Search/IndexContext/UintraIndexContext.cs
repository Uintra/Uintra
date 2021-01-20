using System.Threading.Tasks;
using Compent.Shared.Search.Contract;
using Compent.Shared.Search.Elasticsearch;
using Compent.Shared.Search.Elasticsearch.Providers;
using Nest;
using Uintra.Core.Search.Providers;

namespace Uintra.Core.Search
{
    public class UintraIndexContext<T> : IIndexContext<T> where T : class, ISearchDocument
    {
        private readonly IElasticClient client;
        private readonly PropertiesDescriptor<T> mapping;
        private readonly INormalizerProvider normalizerProvider;
        private readonly ICharFiltersProvider charFiltersProvider;
        private readonly IAnalyzerProvider analyzerProvider;
        private readonly IFiltersProvider filterProvider;
        private readonly ITokenizerProvider tokenizerProvider;

        public SearchIndexName IndexName { get; }

        private Indices Indices { get; }

        public UintraIndexContext(
            IElasticClient client,
            PropertiesDescriptor<T> mapping,
            IndexHelper indexHelper,
            INormalizerProvider normalizerProvider,
            ICharFiltersProvider charFiltersProvider,
            IAnalyzerProvider analyzerProvider,
            IFiltersProvider filterProvider, ITokenizerProvider tokenizerProvider)
        {
            this.client = client;
            this.mapping = mapping;
            this.normalizerProvider = normalizerProvider;
            this.charFiltersProvider = charFiltersProvider;
            this.analyzerProvider = analyzerProvider;
            this.filterProvider = filterProvider;
            this.tokenizerProvider = tokenizerProvider;

            Indices = indexHelper.GetIndices<T>();
            IndexName = new SearchIndexName(indexHelper.GetIndexName<T>().Name);
        }

        public virtual async Task<SearchResponse> RecreateIndex()
        {
            var indexExistedResponse = await client.Indices.ExistsAsync(Indices).ConfigureAwait(false);
            if (indexExistedResponse.IsFail()) return indexExistedResponse.ToSearchResponse();

            if (indexExistedResponse.Exists)
            {
                var deleteResponse = await client.Indices.DeleteAsync(Indices).ConfigureAwait(false);
                if (deleteResponse.IsFail()) return deleteResponse.ToSearchResponse();
            }

            return await CreateIndex().ConfigureAwait(false);
        }

        public virtual async Task<SearchResponse> EnsureIndex()
        {
            var indexExistedResponse = await client.Indices.ExistsAsync(Indices).ConfigureAwait(false);

            if (indexExistedResponse.Exists || indexExistedResponse.IsFail())
                return indexExistedResponse.ToSearchResponse();

            return await CreateIndex().ConfigureAwait(false);
        }

        protected virtual async Task<SearchResponse> CreateIndex()
        {
            var indexResponse = await client.Indices.CreateAsync(Indices.ToIndexName(), descriptor =>
                descriptor.Settings(f => f
                    .NumberOfReplicas(0)
                    .NumberOfShards(1)
                    .Setting(UpdatableIndexSettings.MaxNGramDiff, 50)
                    .Analysis(AggregateAnalysis)))

                .ConfigureAwait(false);

            if (indexResponse.IsFail()) return indexResponse.ToSearchResponse();

            var response = await client
                .MapAsync<T>(descriptor => descriptor.Index(Indices).Properties(pd => mapping))
                .ConfigureAwait(false);

            return response.ToSearchResponse();
        }

        private AnalysisDescriptor AggregateAnalysis(AnalysisDescriptor analysisDescriptor)
        {
            analyzerProvider.Apply(analysisDescriptor);
            normalizerProvider.Apply(analysisDescriptor);
            filterProvider.Apply(analysisDescriptor);
            charFiltersProvider.Apply(analysisDescriptor);
            tokenizerProvider.Apply(analysisDescriptor);

            return analysisDescriptor;
        }
    }
}