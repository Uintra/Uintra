using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using uIntra.Core.Extentions;

namespace uIntra.Search.Core
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
                    .Field(f => f.Description)),
                new QueryContainerDescriptor<SearchableContent>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.PanelContent)),
                new QueryContainerDescriptor<SearchableContent>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.PanelTitle)),
                new QueryContainerDescriptor<SearchableDocument>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.Attachment.Content))
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
                        .PreTags(SearchConstants.HighlightPreTag)
                        .PostTags(SearchConstants.HighlightPostTag))
                );
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
                        documents.Add(SerializationExtentions.Deserialize<SearchableContent>(document.ToString()));
                        break;
                    case SearchableType.Document:
                        documents.Add(SerializationExtentions.Deserialize<SearchableDocument>(document.ToString()));
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

                    case "panelContent":
                        panelContent.AddRange(field.Highlights);
                        break;

                    case "panelTitle":
                        panelContent.AddRange(field.Highlights);
                        break;

                    case "attachment.content":
                        document.attachment.content = highlightedField;
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