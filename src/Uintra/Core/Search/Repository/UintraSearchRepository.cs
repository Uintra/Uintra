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

        public override async Task<string> IndexAsync(T item)
        {
            if (item == null) return default(string);
            
            var response = await client
                .IndexAsync<T>(item, x => x.Index(indexContext.IndexName.Name).Refresh(Refresh.False))
                .ConfigureAwait(false);
            
            return !response.IsFail() && !response.Id.IsEmpty() ? response.Id : default(string);
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

            if (items.Any() && items.First() is SearchableDocument)
                descriptor.Pipeline(SearchConstants.AttachmentsPipelineName).Refresh(Refresh.WaitFor);
            else
                descriptor.Refresh(Refresh.WaitFor);

            var response = await client.BulkAsync(descriptor).ConfigureAwait(false);

            return response.IsValid && response.Items.HasValue() ? response.Items.Count : default(int);
        }
    }

    public class UintraSearchRepository : SearchRepository, IUintraSearchRepository
    {
        private readonly SpecificationAbstractFactory<SearchDocument> searchSpecificationFactory;
        private readonly IElasticClientWrapper client;
        private readonly ILog<SearchRepository> log;
        private readonly IndexHelper indexHelper;
        private readonly ISearchHighlightingHelper searchHighlightingHelper;
        private readonly ISearchDocumentTypeHelper searchDocumentTypeHelper;
        public UintraSearchRepository(
            SpecificationAbstractFactory<SearchDocument> searchSpecificationFactory,
            IElasticClientWrapper client,
            ILog<SearchRepository> log,
            IndexHelper indexHelper,
            ISearchHighlightingHelper searchHighlightingHelper,
            ISearchDocumentTypeHelper searchDocumentTypeHelper)
            : base(searchSpecificationFactory, client, log, indexHelper, searchHighlightingHelper, searchDocumentTypeHelper)
        {
            this.searchSpecificationFactory = searchSpecificationFactory;
            this.client = client;
            this.log = log;
            this.indexHelper = indexHelper;
            this.searchHighlightingHelper = searchHighlightingHelper;
            this.searchDocumentTypeHelper = searchDocumentTypeHelper;
        }

        //public override async Task<ISearchResult<ISearchDocument>> SearchAsync<TQuery>(TQuery query, string culture)
        //{
        //    var specification = searchSpecificationFactory.CreateSearchSpecification(query, culture);
        //    var searchDescriptor = specification.Descriptor.AllIndices();
        //
        //    //searchDescriptor.RequestConfiguration(r => r.DisableDirectStreaming());
        //
        //    var response = await client.SearchAsync<JObject>(searchDescriptor).ConfigureAwait(false);
        //
        //    var res = response.DebugInformation;
        //
        //    if (response.IsFail())
        //    {
        //        LogError(response);
        //        return new Compent.Shared.Search.Contract.SearchResult<ISearchDocument>();
        //    }
        //
        //    return MapToResult(response);
        //}

        //protected override Type GetSearchType(string indexName)
        //{
        //    var clearIndexName = indexHelper.TrimPrefix(indexName);
        //    var type = searchDocumentTypeHelper.Get(clearIndexName);
        //    return type;
        //}

        protected override ISearchResult<ISearchDocument> MapToResult(ISearchResponse<JObject> response)
        {
            var highlightedResponse = searchHighlightingHelper.HighlightResponse(response);

            var documents = highlightedResponse.Hits
                .Select(hit =>
                {
                    var searchType = GetSearchType(hit.Index);
                    return (SearchableBase)hit.Source.ToObject(searchType);
                });

            var result = new Entities.SearchResult<SearchableBase>()
            {
                Documents = documents,
                TotalCount = (int)response.Total,
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