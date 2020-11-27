using System.Collections.Generic;
using System.Linq;
using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Extensions.Bcl;
using Nest;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Helpers;
using Uintra.Features.Search;
using Uintra.Features.Search.Member;

namespace Uintra.Core.Search.Queries.SearchByText
{
    public class SearchByTextSpecification : UBaseline.Search.Elasticsearch.SearchByTextSpecification//: SearchQuerySpecification<SearchableBase>
    {
        protected const int MinimumShouldMatches = 1;
        private readonly IMemberSearchDescriptorBuilder _memberSearchDescriptorBuilder;

        public SearchByTextSpecification(UBaseline.Search.Core.SearchByTextQuery query, IDependencyProvider dependencyProvider, string culture) : base(query, dependencyProvider, culture)
        {
            // TODO: Search. Discuss this
            _memberSearchDescriptorBuilder = dependencyProvider.GetService<IMemberSearchDescriptorBuilder>();
            ApplyAggregations(Descriptor, query as SearchByTextQuery);
        }

        protected override QueryContainer GetSearchByTextQuery(string text)
        {
            // TODO: Search. Check trackScores
            var result  = Query<SearchableBase>.Bool(b => b
                .Should(GetQueryContainers(text))
                .MinimumShouldMatch(MinimumShouldMatch.Fixed(MinimumShouldMatches)));
            
            return result;
        }

        private void ApplyAggregations<T>(SearchDescriptor<T> searchDescriptor, SearchByTextQuery query)
            where T : class
        {
            searchDescriptor.Aggregations(agg => agg
                .Global(SearchConstants.SearchFacetNames.Types, g => g
                    .Aggregations(a => a
                        .Filter(SearchConstants.SearchFacetNames.GlobalFilter, ss => ss
                            .Filter(fi => fi
                                .Bool(b => b
                                    .Should(GetQueryContainers(query.Text))
                                    .Must(GetSearchableTypeQueryContainers(query.SearchableTypeIds))
                                    .Must(GetOnlyPinnedQueryContainer(query.OnlyPinned))
                                ))
                            .Aggregations(
                                ia =>
                                    ia.Terms(SearchConstants.SearchFacetNames.Types,
                                        f => f.Field("type").Size(ElasticHelpers.MaxAggregationSize)))))));
        }

        protected override QueryContainer GetPostFilterQuery(UBaseline.Search.Core.SearchByTextQuery query)
        {
            var extendedQuery = query as SearchByTextQuery;
            var result = Query<SearchableBase>.Bool(b => b.Must(GetSearchPostFilters(extendedQuery)));

            return result;
        }

        protected virtual QueryContainer[] GetQueryContainers(string query)
        {
            var containers = new List<QueryContainer>();
            containers.AddRange(GetBaseDescriptor(query).ToEnumerable());
            containers.AddRange(GetContentDescriptors(query));
            containers.AddRange(GetActivityDescriptor(query).ToEnumerable());
            containers.AddRange(GetDocumentsDescriptor(query).ToEnumerable());
            containers.Add(GetTagNames<SearchableUintraActivity>(query));
            containers.Add(GetTagNames<SearchableMember>(query));
            containers.Add(GetTagsDescriptor(query));
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
                    .Path(p => p.Panels)
                    .Query(q => q
                        .Match(m => m
                            .Query(query)
                            .Field(f => f.Panels.First().Title)))),

                new QueryContainerDescriptor<SearchableContent>()
                    .Nested(nes => nes
                        .Path(p => p.Panels)
                        .Query(q => q
                            .Match(m => m
                                .Query(query)
                                .Field(f => f.Panels.First().Content)))),
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