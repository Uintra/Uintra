using System.Linq;
using Uintra.Core.User;
using Uintra.Groups;

namespace Uintra.Search
{
	public class MembersIndexer : IIndexer
	{
		private readonly IIntranetMemberService<IGroupMember> _intranetMemberService;
		private readonly IElasticMemberIndex _elasticMemberIndex;
		private readonly ISearchableMemberMapper _searchableMemberMapper;

		public MembersIndexer(
			IIntranetMemberService<IGroupMember> intranetMemberService,
			IElasticMemberIndex elasticMemberIndex,
			ISearchableMemberMapper searchableMemberMapper
		)
		{
			_intranetMemberService = intranetMemberService;
			_elasticMemberIndex = elasticMemberIndex;
			_searchableMemberMapper = searchableMemberMapper;
		}
		public void FillIndex()
		{
			var actualUsers = _intranetMemberService.GetAll().Where(u => !u.Inactive).ToList();
			var searchableUsers = actualUsers.Select(_searchableMemberMapper.Map);
			_elasticMemberIndex.Index(searchableUsers);
		}
	}
}
