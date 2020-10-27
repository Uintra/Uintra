using System.Collections.Generic;
using System.Linq;
using Compent.CommandBus;
using Uintra.Core.Commands;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Entities.Mappers;
using Uintra.Core.Search.Indexes;
using Uintra.Features.Groups;

namespace Uintra.Features.Search.CommandBus
{
	public class MemberHandle<T>: IHandle<MemberChanged>, IHandle<MembersChanged> where T : SearchableMember
	{
		private readonly IElasticMemberIndex<T> _elasticMemberIndex;
		private readonly ISearchableMemberMapper<T> _searchableMemberMapper;

		public MemberHandle(IElasticMemberIndex<T> elasticMemberIndex,ISearchableMemberMapper<T> searchableMemberMapper
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