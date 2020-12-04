using System.Linq;
using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Search.Elasticsearch;
using Nest;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Helpers;
using Uintra.Core.Search.Paging;
using Uintra.Core.Search.Sorting;
using Uintra.Features.Search.Member;

namespace Uintra.Core.Search.Queries
{
    public class SearchByMemberSpecification : SearchQuerySpecification<SearchableMember>
    {
        private int MinimumShouldMatches => 1;

        private readonly IMemberSearchDescriptorBuilder _memberSearchDescriptorBuilder;
        private readonly ISearchSortingHelper<SearchableMember> _searchSortingHelper;
        private readonly ISearchPagingHelper<SearchableMember> _searchPagingHelper;

        public SearchByMemberSpecification(SearchByMemberQuery query, IDependencyProvider dependencyProvider, string culture) : base(dependencyProvider, culture)
        {
            _searchSortingHelper = dependencyProvider.GetService<ISearchSortingHelper<SearchableMember>>();
            _memberSearchDescriptorBuilder = dependencyProvider.GetService<IMemberSearchDescriptorBuilder>();
            _searchPagingHelper = dependencyProvider.GetService<ISearchPagingHelper<SearchableMember>>();

            Descriptor = CreateDescriptor(query);

            SortByMemberGroupRights(Descriptor, query);
            _searchPagingHelper.Apply(Descriptor, query);
        }

        private SearchDescriptor<SearchableMember> CreateDescriptor(SearchByMemberQuery query)
        {
            // TODO: Search. Refactor?

            var shouldDescriptor = GetDefaultMemberQueryContainer(query);

            QueryContainer allDescriptors = null;

            if (!query.GroupId.HasValue)
            {
                allDescriptors = new QueryContainerDescriptor<SearchableMember>()
                    .Bool(b => b
                        .Must(shouldDescriptor));
            }
            else
            {
                if (query.MembersOfGroup)
                {
                    allDescriptors = new QueryContainerDescriptor<SearchableMember>()
                        .Bool(b => b
                            .Must(shouldDescriptor, _memberSearchDescriptorBuilder.GetMemberInGroupDescriptor(query.GroupId)));
                }
            }

            var result = GetSearchDescriptor()
                .Query(q =>
                    q.Bool(b => b
                        .Should(allDescriptors)
                        .MinimumShouldMatch(MinimumShouldMatch.Fixed(MinimumShouldMatches))));

            if (query.GroupId.HasValue && !query.MembersOfGroup)
            {
                result = GetSearchDescriptor()
                    .Query(q =>
                        q.Bool(b => b
                            .Should(_memberSearchDescriptorBuilder.GetMemberDescriptors(query.Text))
                            .MinimumShouldMatch(MinimumShouldMatch.Fixed(MinimumShouldMatches))))
                    .PostFilter(pf => pf
                        .Bool(b => b
                            .MustNot(_memberSearchDescriptorBuilder.GetMemberInGroupDescriptor(query.GroupId))));
            }

            return result;
        }

        protected virtual QueryContainer GetDefaultMemberQueryContainer(SearchByMemberQuery query)
        {
            return new QueryContainerDescriptor<SearchableMember>()
                .Bool(b => b.Should(_memberSearchDescriptorBuilder.GetMemberDescriptors(query.Text)));
        }
        protected virtual SearchDescriptor<SearchableMember> GetSearchDescriptor()
        {
            return new SearchDescriptor<SearchableMember>()
                .TrackScores()
                .Index<SearchableMember>();
            // TODO: Search. Index?
        }


        private void SortByMemberGroupRights(SearchDescriptor<SearchableMember> searchDescriptor, SearchByMemberQuery query)
        {
            if (query.GroupId.HasValue)
            {
                searchDescriptor.Sort(s => s
                    .Field(f => f
                        .Field(ff => ff.Groups.First().IsAdmin)
                        .Nested(p => 
                            p.Path(np => np.Groups)
                                .Filter(nf => 
                                    nf.Term(t => 
                                        t.Field(ff => 
                                            ff.Groups.First().GroupId).Value(query.GroupId))))
                        .Descending())
                    .Field($"{ElasticHelpers.FullName}.{ElasticHelpers.Normalizer.Sort}", SortOrder.Ascending));
            }
            else
                _searchSortingHelper.Apply(searchDescriptor, query);
        }
    }
}