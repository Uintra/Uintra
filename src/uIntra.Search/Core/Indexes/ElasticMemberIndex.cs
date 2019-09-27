using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Uintra.Search.Member;
using Uintra.Search.Paging;
using Uintra.Search.Sorting;

namespace Uintra.Search
{
	public class ElasticMemberIndex : IElasticMemberIndex, IElasticEntityMapper
	{
		protected const int MinimumShouldMatches = 1;

		private readonly IElasticSearchRepository<SearchableMember> _elasticSearchRepository;
		private readonly IMemberSearchDescriptorBuilder _memberSearchDescriptorBuilder;
		private readonly ISearchPagingHelper<SearchableMember> _searchPagingHelper;
		private readonly ISearchSortingHelper<SearchableMember> _searchSortingHelper;
		private bool _isMappingChecked;


		public ElasticMemberIndex(
			IElasticSearchRepository<SearchableMember> elasticSearchRepository,
			IMemberSearchDescriptorBuilder memberSearchDescriptorBuilder,
			ISearchPagingHelper<SearchableMember> searchPagingHelper,
			ISearchSortingHelper<SearchableMember> searchSortingHelper
		)
		{
			_elasticSearchRepository = elasticSearchRepository;
			_memberSearchDescriptorBuilder = memberSearchDescriptorBuilder;
			_searchPagingHelper = searchPagingHelper;
			_searchSortingHelper = searchSortingHelper;
		}

		private SearchDescriptor<SearchableMember> GetSearchDescriptor()
		{
			return new SearchDescriptor<SearchableMember>()
				.TrackScores()
				.Type<SearchableMember>();
		}

		public void Index(SearchableMember member)
		{
			EnsureMappingExist();
			_elasticSearchRepository.Save(member);
		}

		public void Index(IEnumerable<SearchableMember> members)
		{
			EnsureMappingExist();
			_elasticSearchRepository.Save(members);
		}

		public SearchResult<SearchableMember> Search(MemberSearchQuery query)
		{
			var shouldDescriptor = new QueryContainerDescriptor<SearchableMember>()
				.Bool(b => b.Should(_memberSearchDescriptorBuilder.GetMemberDescriptors(query.Text)));

			QueryContainer allDescriptors;
			if (query.MembersOfGroup)
			{
				allDescriptors = new QueryContainerDescriptor<SearchableMember>()
					.Bool(b => b
						.Must(shouldDescriptor, _memberSearchDescriptorBuilder.GetMemberInGroupDescriptor(query.GroupId)));
			}
			else
			{
				allDescriptors = new QueryContainerDescriptor<SearchableMember>()
					.Bool(b => b
						.Must(shouldDescriptor)
							.MustNot(_memberSearchDescriptorBuilder.GetMemberInGroupDescriptor(query.GroupId)));
			}

			var searchRequest = GetSearchDescriptor()
				.Query(q =>
					q.Bool(b => b
						.Should(allDescriptors)
						.MinimumShouldMatch(MinimumShouldMatch.Fixed(MinimumShouldMatches))));

			SortByMemberGroupRights(searchRequest, query);

			_searchPagingHelper.Apply(searchRequest, query);

			var queryResult = _elasticSearchRepository.SearchByIndex(searchRequest);
			var searchResult = ParseResults(queryResult);
			return searchResult;
		}
		public void Delete(Guid id)
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

		public bool CreateMap(out string error)
		{
			return _elasticSearchRepository.CreateMap(out error);
		}

		protected virtual SearchResult<SearchableMember> ParseResults(ISearchResponse<SearchableMember> response)
		{
			var result = new SearchResult<SearchableMember>
			{
				TotalHits = response.Total,
				Documents = response.Documents,
			};

			return result;
		}

		protected virtual void SortByMemberGroupRights(SearchDescriptor<SearchableMember> searchDescriptor, MemberSearchQuery query)
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