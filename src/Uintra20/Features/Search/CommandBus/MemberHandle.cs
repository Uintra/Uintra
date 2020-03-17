using System.Collections.Generic;
using System.Linq;
using Compent.CommandBus;
using Uintra20.Core.Commands;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Entities.Mappers;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.Groups;

namespace Uintra20.Features.Search.CommandBus
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