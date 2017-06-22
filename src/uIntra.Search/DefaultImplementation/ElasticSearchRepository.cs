using System;
using System.Collections.Generic;
using Nest;
using uIntra.Core.Extentions;
using uIntra.Search.Core;
using uIntra.Search.Core.Configuration;
using uIntra.Search.Core.Entities;
using IExceptionLogger = uIntra.Core.Exceptions.IExceptionLogger;

namespace uIntra.Search
{
    public class ElasticSearchRepository : IElasticSearchRepository
    {
        protected readonly IElasticConfigurationSection Configuration;
        protected readonly IElasticClient Client;
        protected readonly string IndexName;

        private readonly IExceptionLogger _exceptionLogger;

        public ElasticSearchRepository(
            string indexName,
            IElasticConfigurationSection configuration,
            IExceptionLogger exceptionLogger)
        {
            Configuration = configuration;
            _exceptionLogger = exceptionLogger;
            IndexName = $"{configuration.IndexPrefix}{indexName.ToLower()}";

            var connectionSettings = new ConnectionSettings(new Uri(configuration.Url)).DefaultIndex(IndexName);
            Client = new ElasticClient(connectionSettings);
        }

        public ISearchResponse<T> SearchByIndex<T>(SearchDescriptor<T> descriptor)
            where T : class
        {
            descriptor.Index(IndexName).TrackScores();
            return GetSearchResponse(descriptor);
        }

        public void EnsureIndexExist(Func<AnalysisDescriptor, AnalysisDescriptor> analysis)
        {
            if (Client.IndexExists(IndexName).Exists) return;

            var createIndexResponse = Client.CreateIndex(IndexName, c => c
                .Index(IndexName).Settings(s => s.NumberOfShards(Configuration.NumberOfShards).NumberOfReplicas(Configuration.NumberOfReplicas).Analysis(analysis))
                );

            if (!createIndexResponse.IsValid)
            {
                RequestError(createIndexResponse);
            }
        }

        public void DeleteIndex()
        {
            if (!Client.IndexExists(IndexName).Exists) return;

            var deleteIndexRequest = Client.DeleteIndex(IndexName);

            if (!deleteIndexRequest.IsValid)
            {
                RequestError(deleteIndexRequest);
            }
        }

        protected ISearchResponse<T> GetSearchResponse<T>(SearchDescriptor<T> descriptor)
             where T : class
        {
            var searchRequest = Client.Search<T>(descriptor);

            if (!searchRequest.IsValid)
            {
                RequestError(searchRequest);
            }

            return searchRequest;
        }

        protected void RequestError(IResponse response)
        {
            _exceptionLogger.Log(response.OriginalException);
        }
    }

    public class ElasticSearchRepository<T> : ElasticSearchRepository, IElasticSearchRepository<T>
        where T : SearchableBase
    {
        private readonly PropertiesDescriptor<T> _properties;

        public ElasticSearchRepository(
            string indexName,
            IElasticConfigurationSection configuration,
            PropertiesDescriptor<T> properties,
            IExceptionLogger exceptionLogger
            )
            : base(indexName, configuration, exceptionLogger)
        {
            _properties = properties;
        }

        public T Get(object id)
        {
            var response = Client.Get<T>(id.ToString(), i => i.Index(IndexName).Type(GetTypeName()));
            return response?.Source;
        }

        public ISearchResponse<T> Search(SearchDescriptor<T> descriptor)
        {
            descriptor.Index(IndexName).Type(GetTypeName()).TrackScores();
            return GetSearchResponse(descriptor);
        }

        public void Save(T document)
        {
            var response = Client.Index(document, i => i.Index(IndexName).Type(GetTypeName()));
            if (!response.IsValid)
            {
                RequestError(response);
            }
        }

        public void Save(IEnumerable<T> documents)
        {
            foreach (var entity in documents.Split(Configuration.LimitBulkOperation))
            {
                var closure = entity;
                var bulkResponse = Client.Bulk(b => b
                    .Index(IndexName).Type(GetTypeName())
                    .IndexMany(closure));

                if (!bulkResponse.IsValid)
                {
                    RequestError(bulkResponse);
                }
            }
        }

        public void DeleteAllByType(SearchableType type)
        {
            var deleteQuery = new DeleteByQueryDescriptor<T>(Indices.Parse(IndexName))
                .Type(Types.Parse(GetTypeName()))
                .Query(q => q.Term(t => t.Field(f => f.Type).Value(type)));
            var response = Client.DeleteByQuery(deleteQuery);

            if (!response.IsValid)
            {
                RequestError(response);
            }
        }

        public void Delete(object id)
        {
            var response = Client.Delete(new DeleteRequest(IndexName, GetTypeName(), id.ToString()));

            if (!response.IsValid)
            {
                RequestError(response);
            }
        }

        public void EnsureMappingExist()
        {
            if (Client.TypeExists(IndexName, GetTypeName()).Exists) return;

            var putMappingResponse = Client.Map<T>(d => d
                .Index(IndexName)
                .Type(GetTypeName())
                .Properties(p => _properties));

            if (!putMappingResponse.IsValid)
            {
                RequestError(putMappingResponse);
            }
        }

        public string GetTypeName()
        {
            return typeof(T).Name.ToLower();
        }
    }
}