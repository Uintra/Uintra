using System.Collections.Generic;
using System.Linq;
using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Extensions.Bcl;
using Compent.Shared.Search.Contract;
using Compent.Shared.Search.Elasticsearch;
using Compent.Shared.Search.Elasticsearch.SearchHighlighting;
using Nest;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Helpers;
using Uintra.Features.Search;
using Uintra.Features.Search.Member;

namespace Uintra.Core.Search.Queries
{
    public class SearchByTextSpecification : SearchQuerySpecification<SearchDocument>
    {
        protected const int MinimumShouldMatches = 1;
        private readonly IMemberSearchDescriptorBuilder _memberSearchDescriptorBuilder;
        private readonly ISearchHighlightingHelper _searchHighlightingHelper;
        private readonly IElasticsearchSettings _elasticsearchSettings;

        public SearchByTextSpecification(SearchByTextQuery query, IDependencyProvider dependencyProvider, string culture): base(dependencyProvider, culture)
        {
            _memberSearchDescriptorBuilder = dependencyProvider.GetService<IMemberSearchDescriptorBuilder>();
            _searchHighlightingHelper = dependencyProvider.GetService<ISearchHighlightingHelper>();
            _elasticsearchSettings = dependencyProvider.GetService<IElasticsearchSettings>();
            Descriptor = CreateDescriptor(query);
            ApplyAggregations(Descriptor);
        }

        private SearchDescriptor<SearchDocument> GetSearchDescriptor()
        {
            return new SearchDescriptor<SearchDocument>()
                .TrackScores()
                .AllIndices();
        }
        private SearchDescriptor<SearchDocument> CreateDescriptor<TSearchByTextQuery>(TSearchByTextQuery query) where TSearchByTextQuery : SearchByTextQuery
        {
            var descriptor = GetSearchDescriptor()
                .Query(q => q
                    .Bool(b => b
                        .Should(GetQueryContainers(query.Text))
                        //.Must(GetPostFilterQuery(query))
                        .MinimumShouldMatch(MinimumShouldMatch.Fixed(MinimumShouldMatches))))
                //.PostFilter(pf => pf.Bool(b => b.Must(GetSearchPostFilters(query))));
                .PostFilter(p => GetPostFilterQuery(query))
                .Sort(GetSortDescriptor)
                .SetPaging(query.Skip, query.Take)
                
                ;
            if (query.ApplyHighlights)
            {
                descriptor = ApplyHighlight(descriptor);
            }

            return descriptor;
        }

        protected override SearchDescriptor<SearchDocument> ApplyHighlight(SearchDescriptor<SearchDocument> descriptor)
        {
            return descriptor.Highlight(h => h
                .PreTags(_searchHighlightingHelper.HighlightPreTag)
                .PostTags(_searchHighlightingHelper.HighlightPostTag)
                .Encoder(HighlighterEncoder.Default)
                .FragmentSize(_elasticsearchSettings.HighlightedFragmentSize)
                .Fields(f => f.Field("*"))
            );
        }

        //private QueryContainer GetSearchByTextQuery(string text)
        //{
        //    var result  = Query<SearchableBase>.Bool(b => b
        //        .Should(GetQueryContainers(text))
        //        .MinimumShouldMatch(MinimumShouldMatch.Fixed(MinimumShouldMatches)));
        //    
        //    return result;
        //}

        private void ApplyAggregations(SearchDescriptor<SearchDocument> searchDescriptor)
        {
            searchDescriptor.Aggregations(ia => ia
                .Terms(SearchConstants.SearchFacetNames.Types, f => f
                    .Field("type").Size(ElasticHelpers.MaxAggregationSize)));
        }

        private QueryContainer GetPostFilterQuery(SearchByTextQuery query)
        {
            var result = Query<SearchableBase>.Bool(b => b.Must(GetSearchPostFilters(query)));
        
            return result;
        }

        protected virtual QueryContainer[] GetQueryContainers(string query)
        {
            var containers = new List<QueryContainer>();
            containers.AddRange(GetBaseDescriptor(query).ToEnumerable());
            containers.AddRange(GetContentDescriptors(query));
            containers.AddRange(GetActivityDescriptor(query).ToEnumerable());
            containers.AddRange(GetDocumentsDescriptor(query).ToEnumerable());
            containers.Add(GetTagNames<SearchableActivity>(query));
            containers.Add(GetTagNames<SearchableMember>(query));
            //containers.Add(GetTagsDescriptor(query));
            containers.AddRange(_memberSearchDescriptorBuilder.GetMemberDescriptors(query));

            return containers.ToArray();
        }

        private QueryContainer GetBaseDescriptor(string query)
        {
            var desc = new QueryContainerDescriptor<SearchableBase>().Match(m => m
                .Query(query)
                .Analyzer(ElasticHelpers.Replace)
                .Field(f => f.Title));
            return desc;
        }

        private QueryContainer[] GetContentDescriptors(string query)
        {
            var desc = new List<QueryContainer>
            {
                new QueryContainerDescriptor<SearchableContent>().Nested(nes => nes
                    .Path(x => x.Panels)
                    .Query(q => q
                        .Match(m => m
                            .Query(query)
                            .Analyzer(ElasticHelpers.ReplaceNgram)
                            .Field(f => f.Panels.First().Title)))),

                new QueryContainerDescriptor<SearchableContent>()
                    .Nested(nes => nes
                        .Path(x => x.Panels)
                        .Query(q => q
                            .Match(m => m
                                .Query(query)
                                .Analyzer(ElasticHelpers.ReplaceNgram)
                                .Field(f => f.Panels.First().Content))))
            };

            return desc.ToArray();
        }

        private QueryContainer GetActivityDescriptor(string query)
        {
            var desc = new QueryContainerDescriptor<SearchableActivity>().Match(m => m
                .Query(query)
                .Analyzer(ElasticHelpers.Replace)
                .Field(f => f.Description));
            return desc;
        }

        private QueryContainer GetDocumentsDescriptor(string query)
        {
            var desc = new QueryContainerDescriptor<SearchableDocument>().Match(m => m
                .Query(query)
                .Analyzer(ElasticHelpers.Replace)
                .Field(f => f.Attachment.Content));
            return desc;
        }

        private QueryContainer[] GetSearchPostFilters(SearchByTextQuery query)
        {
            var result =  new[]
            {
                GetSearchableTypeQueryContainers(query.SearchableTypeIds),
                GetOnlyPinnedQueryContainer(query.OnlyPinned)
            };

            if (query.SearchableTypeIds.Count() == 1 && query.SearchableTypeIds.First() == (int)UintraSearchableTypeEnum.Member)
                return result
                    .Append(new QueryContainerDescriptor<SearchableMember>().Terms(t => t.Field(f => f.Inactive).Terms(false)))
                    .ToArray();

            return result;

        }

        private QueryContainer GetSearchableTypeQueryContainers(IEnumerable<int> searchableTypeIds)
        {
            return new QueryContainerDescriptor<SearchableBase>().Terms(t =>
                t.Field(f => f.Type).Terms(searchableTypeIds));
        }

        private QueryContainer GetOnlyPinnedQueryContainer(bool onlyPinned)
        {
            var result = onlyPinned
                ? new QueryContainerDescriptor<SearchableActivity>().Terms(t => t.Field(f => f.IsPinActual).Terms(true))
                : new QueryContainerDescriptor<SearchableActivity>();
            return result;
        }

        protected static QueryContainer GetTagNames<T>(string query) where T : class, ISearchableTaggedActivity
        {
            var desc = new QueryContainerDescriptor<T>().Match(m => m
                .Query(query)
                .Field(f => f.UserTagNames));
            return desc;
        }
        
        public QueryContainer GetTagsDescriptor(string query)
        {
            var desc = new QueryContainerDescriptor<SearchableTag>().Match(m => m
                .Query(query)
                .Field(f => f.Title));
            return desc;
        }
    }
}