using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using uIntra.Core;
using uIntra.Core.Extentions;
using uIntra.Search.Core.Entities;
using uIntra.Search.Core.Queries;

namespace uIntra.Search.Core.Indexes
{
    public class ElasticIndex : IElasticIndex
    {
        private readonly IElasticSearchRepository _elasticSearchRepository;

        public ElasticIndex(
            IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public SearchResult<SearchableBase> Search(SearchTextQuery textQuery)
        {
            var searchRequest = new SearchDescriptor<dynamic>()
                .AllTypes()
                .TrackScores()
                .Query(q =>
                    q.Bool(b => b
                        .Should(
                            GetQueryContainers(textQuery.Text)
                        )))
                .Take(textQuery.Take);

            ApplySort(searchRequest);

            if (textQuery.ApplyHighlights)
            {
                ApplyHighlight(searchRequest);
            }

            var queryResult = _elasticSearchRepository.SearchByIndex(searchRequest);
            var searchResult = ParseResults(queryResult);
            return searchResult;
        }

        public void RecreateIndex()
        {
            _elasticSearchRepository.DeleteIndex();
            _elasticSearchRepository.EnsureIndexExist(ElasticHelpers.SetAnalysis);
        }

        private static QueryContainer[] GetQueryContainers(string query)
        {
            return new[]
            {
                new QueryContainerDescriptor<SearchableBase>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.Title)),
                new QueryContainerDescriptor<SearchableActivity>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.Teaser)),
                new QueryContainerDescriptor<SearchableActivity>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.Description)),
                new QueryContainerDescriptor<SearchableContent>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.PanelContent)),
                new QueryContainerDescriptor<SearchableContent>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.PanelTitle))
            };
        }

        private static SearchResult<SearchableBase> ParseResults(ISearchResponse<dynamic> response)
        {
            var searchResult = new SearchResult<SearchableBase>
            {
                TotalHits = response.Total,
                Facets = response.Aggs.Aggregations,
            };

            var results = ParseDocuments<SearchableBase>(response, true);
            searchResult.Documents = results;

            return searchResult;
        }

        private static void ApplyHighlight<T>(SearchDescriptor<T> searchRequest)
            where T : class
        {
            searchRequest
                .Highlight(hd => hd
                    .Fields(ff => ff
                        .Field("*")
                        .PreTags(IntranetConstants.SearchConstants.HighlightPreTag)
                        .PostTags(IntranetConstants.SearchConstants.HighlightPostTag))

                );
        }


        private static void ApplyPaging(ISearchRequest searchRequest, int page, int pageSize)
        {
            searchRequest.From = page - 1;
            searchRequest.Size = pageSize;
        }

        private void ApplySort<T>(SearchDescriptor<T> searchDescriptor)
            where T : class
        {
            searchDescriptor.Sort(s => s.Descending("_score"));
        }

        private static List<T> ParseDocuments<T>(ISearchResponse<dynamic> response, bool applyHighlight = false)
            where T : SearchableBase
        {
            if (applyHighlight)
            {
                var highlights = response.Hits.ToDictionary(x => x.Id, x => x.Highlights).ToList();

                foreach (var document in response.Documents)
                {
                    var highlight = highlights.Find(el => el.Key == document.id.ToString());
                    if (highlight.Key == null)
                    {
                        continue;
                    }
                    Highlight(document, highlight.Value);
                }

            }

            var documents = new List<T>();
            foreach (var document in response.Documents)
            {
                switch ((SearchableType)document.type)
                {
                    case SearchableType.Events:
                    case SearchableType.Ideas:
                    case SearchableType.News:
                        documents.Add(SerializationExtentions.Deserialize<SearchableActivity>(document.ToString()));
                        break;
                    case SearchableType.Content:
                    case SearchableType.Document:
                        documents.Add(SerializationExtentions.Deserialize<SearchableContent>(document.ToString()));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Could not resolve type of searchable entity {(SearchableType)document.type}");
                }
            }
            return documents;
        }

        private static void Highlight(dynamic document, Dictionary<string, HighlightHit> fields)
        {
            var panelContent = new List<string>();

            foreach (var field in fields.Values)
            {
                var highlightedField = field.Highlights.FirstOrDefault();
                switch (field.Field)
                {
                    case "title":
                        document.title = highlightedField;
                        break;
                    case "description":
                        document.description = highlightedField;
                        break;

                    case "teaser":
                        document.teaser = highlightedField;
                        break;

                    case "panelContent":
                        panelContent.AddRange(field.Highlights);
                        break;

                    case "panelTitle":
                        panelContent.AddRange(field.Highlights);
                        break;
                }
            }

            if (panelContent.Any())
            {
                document.panelContent = panelContent.ToDynamic();
            }
        }
    }
}