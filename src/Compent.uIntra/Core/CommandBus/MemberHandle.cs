using System.Collections.Generic;
using System.Linq;
using Compent.CommandBus;
using Uintra.Groups;
using Uintra.Search;
using Uintra.Users.Commands;

namespace Compent.Uintra.Core.CommandBus
{
	public class MemberHandle: IHandle<MemberChanged>, IHandle<MembersChanged>
	{
		private readonly IElasticMemberIndex _elasticMemberIndex;
		private readonly ISearchableMemberMapper _searchableMemberMapper;

		public MemberHandle(IElasticMemberIndex elasticMemberIndex,ISearchableMemberMapper searchableMemberMapper
		)
		{
			_elasticMemberIndex = elasticMemberIndex;
			_searchableMemberMapper = searchableMemberMapper;
		}

		public BroadcastResult Handle(MemberChanged command)
		{
			_elasticMemberIndex.Index(_searchableMemberMapper.Map(command.Member as IGroupMember));
			return BroadcastResult.Success;
		}

		public BroadcastResult Handle(MembersChanged command)
		{
			var members = command.Members as IEnumerable<IGroupMember>;
			var searchableMembers = members?.Select(_searchableMemberMapper.Map);
			_elasticMemberIndex.Index(searchableMembers);
			return BroadcastResult.Success;
		}
	}
}