using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Uintra.Search.Member;
using Uintra.Search.Paging;
using Uintra.Search.Sorting;

namespace Uintra.Search
{
    public class ElasticMemberIndex<T> : IElasticMemberIndex<T>, IElasticEntityMapper where T : SearchableMember
    {
        protected virtual int MinimumShouldMatches => 1;

        private readonly IElasticSearchRepository<T> _elasticSearchRepository;
        private readonly IMemberSearchDescriptorBuilder _memberSearchDescriptorBuilder;
        private readonly ISearchPagingHelper<T> _searchPagingHelper;
        private readonly ISearchSortingHelper<T> _searchSortingHelper;
        private bool _isMappingChecked;

        public ElasticMemberIndex(
            IElasticSearchRepository<T> elasticSearchRepository,
            IMemberSearchDescriptorBuilder memberSearchDescriptorBuilder,
            ISearchPagingHelper<T> searchPagingHelper,
            ISearchSortingHelper<T> searchSortingHelper
        )
        {
            _elasticSearchRepository = elasticSearchRepository;
            _memberSearchDescriptorBuilder = memberSearchDescriptorBuilder;
            _searchPagingHelper = searchPagingHelper;
            _searchSortingHelper = searchSortingHelper;
        }

        protected virtual SearchDescriptor<T> GetSearchDescriptor()
        {
            return new SearchDescriptor<T>()
                .TrackScores()
                .Type<T>();
        }

        public virtual void Index(T member)
        {
            EnsureMappingExist();
            _elasticSearchRepository.Save(member);
        }

        public virtual void Index(IEnumerable<T> members)
        {
            EnsureMappingExist();
            _elasticSearchRepository.Save(members);
        }

        protected virtual QueryContainer GetDefaultMemberQueryContainer(MemberSearchQuery query)
        {
            return new QueryContainerDescriptor<T>()
                .Bool(b => b.Should(_memberSearchDescriptorBuilder.GetMemberDescriptors(query.Text)));
        }

        public virtual SearchResult<T> Search(MemberSearchQuery query)
        {
            var shouldDescriptor = GetDefaultMemberQueryContainer(query);

            QueryContainer allDescriptors = null;

            if (!query.GroupId.HasValue)
            {
                allDescriptors = new QueryContainerDescriptor<T>()
                    .Bool(b => b
                        .Must(shouldDescriptor));
            }
            else
            {
                if (query.MembersOfGroup)
                {
                    allDescriptors = new QueryContainerDescriptor<T>()
                        .Bool(b => b
                            .Must(shouldDescriptor, _memberSearchDescriptorBuilder.GetMemberInGroupDescriptor(query.GroupId)));
                }
            }

            var searchRequest = GetSearchDescriptor()
                .Query(q =>
                    q.Bool(b => b
                        .Should(allDescriptors)
                        .MinimumShouldMatch(MinimumShouldMatch.Fixed(MinimumShouldMatches))));

            if (query.GroupId.HasValue && !query.MembersOfGroup)
            {
                searchRequest = GetSearchDescriptor()
                    .Query(q =>
                        q.Bool(b => b
                            .Should(_memberSearchDescriptorBuilder.GetMemberDescriptors(query.Text))
                            .MinimumShouldMatch(MinimumShouldMatch.Fixed(MinimumShouldMatches))))
                    .PostFilter(pf => pf
                        .Bool(b => b
                            .MustNot(_memberSearchDescriptorBuilder.GetMemberInGroupDescriptor(query.GroupId))));
            }

            SortByMemberGroupRights(searchRequest, query);

            _searchPagingHelper.Apply(searchRequest, query);

            var queryResult = _elasticSearchRepository.SearchByIndex(searchRequest);
            var searchResult = ParseResults(queryResult);
            return searchResult;
        }

        public virtual void Delete(Guid id)
        {
            _elasticSearchRepository.Delete(id);
        }

        private void EnsureMappingExist()
        {
            if (_isMappingChecked)
            {
                return;
            }

            _elasticSearchRepository.EnsureMappingExist();
            _isMappingChecked = true;
        }

        public virtual bool CreateMap(out string error)
        {
            return _elasticSearchRepository.CreateMap(out error);
        }

        protected virtual SearchResult<T> ParseResults(ISearchResponse<T> response)
        {
            var result = new SearchResult<T>
            {
                TotalHits = response.Total,
                Documents = response.Documents,
            };

            return result;
        }

        protected virtual void SortByMemberGroupRights(SearchDescriptor<T> searchDescriptor, MemberSearchQuery query)
        {
            _searchSortingHelper.Apply(searchDescriptor, query);
            if (query.GroupId.HasValue)
            {
                searchDescriptor.Sort(s => s
                    .Field(f => f
                        .Field(ff => ff.Groups.First().IsAdmin)
                        .NestedPath(np => np.Groups)
                        .NestedFilter(nf => nf.Term(t => t.Field(ff => ff.Groups.First().GroupId).Value(query.GroupId)))
                        .Descending()
                    ));
            }

        }
    }
}