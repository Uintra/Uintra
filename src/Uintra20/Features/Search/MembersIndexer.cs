using System.Linq;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Search.Entities;
using Uintra20.Features.Search.Indexes;

namespace Uintra20.Features.Search
{
	public class MembersIndexer<T> : IIndexer where T : SearchableMember
	{
		private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
		private readonly IElasticMemberIndex<T> _elasticMemberIndex;
		private readonly ISearchableMemberMapper<T> _searchableMemberMapper;

		public MembersIndexer(
			IIntranetMemberService<IntranetMember> intranetMemberService,
			IElasticMemberIndex<T> elasticMemberIndex,
			ISearchableMemberMapper<T> searchableMemberMapper
		)
		{
			_intranetMemberService = intranetMemberService;
			_elasticMemberIndex = elasticMemberIndex;
			_searchableMemberMapper = searchableMemberMapper;
		}
		public virtual void FillIndex()
		{
			var actualUsers = _intranetMemberService.GetAll().Where(u => !u.Inactive).ToList();
			var searchableUsers = actualUsers.Select(_searchableMemberMapper.Map);
			_elasticMemberIndex.Index(searchableUsers);
		}
	}
}
