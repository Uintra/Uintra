using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using uIntra.Core.Extentions;

namespace uIntra.Search
{
    public class ElasticIndex : IElasticIndex
    {
        private const int MinimumShouldMatches = 1;

        private readonly IElasticSearchRepository _elasticSearchRepository;

        public ElasticIndex(IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        public SearchResult<SearchableBase> Search(SearchTextQuery query)
        {
            var searchRequest = GetSearchDescriptor()
                .Query(q =>
                    q.Bool(b => b
                       .Should(GetQueryContainers(query.Text))
                       .MinimumShouldMatch(MinimumShouldMatch.Fixed(MinimumShouldMatches))))
                       .PostFilter(pf => pf.Bool(b => b.Must(GetSearchableTypeQueryContainers(query.SearchableTypeIds), GetOnlyPinnedQueryContainer(query.OnlyPinned))))
                       .Take(query.Take);

            ApplySort(searchRequest);

            if (query.ApplyHighlights)
            {
                ApplyHighlight(searchRequest);
            }

            var queryResult = _elasticSearchRepository.SearchByIndex(searchRequest);
            var searchResult = ParseResults(queryResult);
            return searchResult;
        }

        protected virtual SearchDescriptor<dynamic> GetSearchDescriptor()
        {
            return new SearchDescriptor<dynamic>()
                .TrackScores()
                .AllTypes();
        }

        public void RecreateIndex()
        {
            _elasticSearchRepository.DeleteIndex();
            _elasticSearchRepository.EnsureIndexExists(ElasticHelpers.SetAnalysis);
        }

        protected virtual QueryContainer GetBaseDescriptor(string query)
        {
            var desc = new QueryContainerDescriptor<SearchableBase>().Match(m => m
                 .Query(query)
                 .Analyzer(ElasticHelpers.Replace)
                 .Field(f => f.Title));
            return desc;
        }

        protected virtual QueryContainer[] GetContentDescriptors(string query)
        {
            var desc = new List<QueryContainer>
            {
                new QueryContainerDescriptor<SearchableContent>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.PanelContent)),
                new QueryContainerDescriptor<SearchableContent>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.PanelTitle))
            };

            return desc.ToArray();
        }

        protected virtual QueryContainer GetActivityDescriptor(string query)
        {
            var desc = new QueryContainerDescriptor<SearchableActivity>().Match(m => m
                .Query(query)
                .Analyzer(ElasticHelpers.Replace)
                .Field(f => f.Description));
            return desc;
        }

        protected virtual QueryContainer GetDocumentsDescriptor(string query)
        {
            var desc = new QueryContainerDescriptor<SearchableDocument>().Match(m => m
                 .Query(query)
                 .Analyzer(ElasticHelpers.Replace)
                 .Field(f => f.Attachment.Content));
            return desc;
        }

        protected virtual QueryContainer[] GetQueryContainers(string query)
        {
            var containers = new List<QueryContainer>();
            containers.AddRange(GetBaseDescriptor(query).ToEnumerableOfOne());
            containers.AddRange(GetContentDescriptors(query));
            containers.AddRange(GetActivityDescriptor(query).ToEnumerableOfOne());
            containers.AddRange(GetDocumentsDescriptor(query).ToEnumerableOfOne());
            return containers.ToArray();
        }

        private static QueryContainer GetSearchableTypeQueryContainers(IEnumerable<int> searchableTypeIds)
        {
            return new QueryContainerDescriptor<SearchableBase>().Terms(t => t.Field(f => f.Type).Terms(searchableTypeIds));
        }

        private QueryContainer GetOnlyPinnedQueryContainer(bool onlyPinned)
        {
            var result = onlyPinned
                ? new QueryContainerDescriptor<SearchableActivity>().Terms(t => t.Field(f => f.IsPinActual).Terms(true))
                : new QueryContainerDescriptor<SearchableActivity>();
            return result;
        }

        protected SearchResult<SearchableBase> ParseResults(ISearchResponse<dynamic> response)
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

        protected virtual void HighlightResponse(ISearchResponse<dynamic> response)
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

        protected virtual List<T> CollectDocuments<T>(ISearchResponse<dynamic> response)
            where T : SearchableBase
        {
            var documents = new List<T>();
            foreach (var document in response.Documents)
            {
                switch ((int)document.type)
                {
                    case (int)SearchableTypeEnum.Events:
                    case (int)SearchableTypeEnum.News:
                    case (int)SearchableTypeEnum.Bulletins:
                        documents.Add(SerializationExtentions.Deserialize<SearchableActivity>(document.ToString()));
                        break;
                    case (int)SearchableTypeEnum.Content:
                        documents.Add(SerializationExtentions.Deserialize<SearchableContent>(document.ToString()));
                        break;
                    case (int)SearchableTypeEnum.Document:
                        documents.Add(SerializationExtentions.Deserialize<SearchableDocument>(document.ToString()));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Could not resolve type of searchable entity {(SearchableTypeEnum)document.type}");
                }
            }
            return documents;
        }

        protected virtual List<T> ParseDocuments<T>(ISearchResponse<dynamic> response, bool applyHighlight = false)
            where T : SearchableBase
        {
            if (applyHighlight)
            {
                HighlightResponse(response);
            }

            return CollectDocuments<T>(response);
        }

        protected virtual void HighlightAdditional(dynamic document, Dictionary<string, HighlightHit> fieldsm,
            List<string> panelContent)
        {
        }

        protected virtual void Highlight(dynamic document, Dictionary<string, HighlightHit> fields)
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

            HighlightAdditional(document, fields, panelContent);

            if (panelContent.Any())
            {
                document.panelContent = panelContent.ToDynamic();
            }
        }
    }
}