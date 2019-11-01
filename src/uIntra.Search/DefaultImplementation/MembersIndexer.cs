using System.Linq;
using Uintra.Core.User;
using Uintra.Groups;

namespace Uintra.Search
{
	public class MembersIndexer<T> : IIndexer where T : SearchableMember
	{
		private readonly IIntranetMemberService<IGroupMember> _intranetMemberService;
		private readonly IElasticMemberIndex<T> _elasticMemberIndex;
		private readonly ISearchableMemberMapper<T> _searchableMemberMapper;

		public MembersIndexer(
			IIntranetMemberService<IGroupMember> intranetMemberService,
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
