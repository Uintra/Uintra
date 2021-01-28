using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.Extensions;
using Compent.Shared.Logging.Contract;
using Compent.Shared.Search.Contract;
using Compent.Shared.Search.Contract.Helpers;
using Compent.Shared.Search.Elasticsearch;
using Compent.Shared.Search.Elasticsearch.SearchHighlighting;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json.Linq;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Extensions;
using Uintra.Core.Search.Queries.DeleteByType;
using Uintra.Features.Search;

namespace Uintra.Core.Search.Repository
{
    public class UintraSearchRepository<T> : SearchRepository<T>, IUintraSearchRepository<T> where T : class, ISearchDocument
    {

        private readonly IIndexContext<T> indexContext;
        private readonly IElasticClientWrapper client;

        protected override string PipelineId => nameof(T);

        public UintraSearchRepository(
            IElasticClientWrapper client,
            IIndexContext<T> indexContext,
            SpecificationAbstractFactory<T> searchSpecificationFactory,
            ILog<SearchRepository> log,
            ISearchHighlightingHelper searchHighlightingHelper)
            : base(client, indexContext, searchSpecificationFactory, log, searchHighlightingHelper)
        {
            this.client = client;
            this.indexContext = indexContext;
        }

        public Task<bool> DeleteByType(UintraSearchableTypeEnum type)
        {
            var query = new DeleteSearchableActivityByTypeQuery
            {
                Type = type
            } as ISearchQuery<T>;
            // TODO: Check this in runtime. Technically it has to be the same type

            return DeleteByQuery(query, string.Empty);
        }

        public override async Task<string> IndexAsync(T item)
        {
            if (item == null) return default;
            
            var response = await client
                .IndexAsync<T>(item, x => x.Index(indexContext.IndexName.Name).Refresh(Refresh.False))
                .ConfigureAwait(false);
            
            return !response.IsFail() && !response.Id.IsEmpty() ? response.Id : default;
        }

        public override async Task<int> IndexAsync(IEnumerable<T> items)
        {
            var itemsList = items.AsList();

            if (itemsList.IsEmpty()) return default(int);

            var descriptor = new BulkDescriptor();

            foreach (var entity in itemsList)
            {
                descriptor.Index<T>(x => x
                    .Id(entity.Id)
                    .Index(indexContext.IndexName.Name)
                    .Document(entity)
                );
            }

            descriptor.Refresh(Refresh.WaitFor);//.Pipeline(PipelineId);


            var response = await client.BulkAsync(descriptor).ConfigureAwait(false);

            return response.IsValid && response.Items.HasValue() ? response.Items.Count : default(int);
        }
    }

    public class UintraSearchRepository : SearchRepository, IUintraSearchRepository
    {
        public UintraSearchRepository(
            SpecificationAbstractFactory<SearchDocument> searchSpecificationFactory,
            IElasticClientWrapper client,
            ILog<SearchRepository> log,
            IndexHelper indexHelper,
            ISearchHighlightingHelper searchHighlightingHelper,
            ISearchDocumentTypeHelper searchDocumentTypeHelper)
            : base(searchSpecificationFactory, client, log, indexHelper, searchHighlightingHelper, searchDocumentTypeHelper)
        {
        }

        protected override ISearchResult<ISearchDocument> MapToResult(ISearchResponse<JObject> response)
        {
            var baseResult = base.MapToResult(response);
            var result = new Entities.SearchResult<ISearchDocument>()
            {
                Documents = baseResult.Documents.OfType<SearchableBase>(), 
                TotalCount = baseResult.TotalCount,
                TypeFacets = response.Aggregations.GetGlobalFacets(SearchConstants.SearchFacetNames.Types)
            };

            return result;
        }

        public async Task<Entities.SearchResult<SearchableBase>> SearchAsyncTyped<TQuery>(TQuery query) where TQuery : ISearchQuery<SearchDocument>
        {
            // TODO: Search. Localization?
            var result = (await base.SearchAsync(query, String.Empty)) as Entities.SearchResult<SearchableBase>;

            return result;
        }
    }
}