using System.Collections.Generic;
using System.Linq;
using Compent.CommandBus;
using Compent.Shared.Extensions.Bcl;
using Uintra.Core.Commands;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Entities.Mappers;
using Uintra.Core.Search.Repository;
using Uintra.Features.Groups;

namespace Uintra.Features.Search.CommandBus
{
	public class MemberHandle<T>: IHandle<MemberChanged>, IHandle<MembersChanged> where T : SearchableMember
	{
        private readonly IUintraSearchRepository<T> _uintraSearchRepository;
		private readonly ISearchableMemberMapper<T> _searchableMemberMapper;

		public MemberHandle(
            ISearchableMemberMapper<T> searchableMemberMapper, 
            IUintraSearchRepository<T> uintraSearchRepository)
		{
			_searchableMemberMapper = searchableMemberMapper;
            _uintraSearchRepository = uintraSearchRepository;
        }

		public BroadcastResult Handle(MemberChanged command)
        {
            var memberToIndex = _searchableMemberMapper.Map(command.Member as IGroupMember);
            AsyncHelpers.RunSync(() => _uintraSearchRepository.IndexAsync(memberToIndex));

			return BroadcastResult.Success;
		}

		public BroadcastResult Handle(MembersChanged command)
		{
			var members = command.Members as IEnumerable<IGroupMember>;
			var searchableMembers = members?.Select(_searchableMemberMapper.Map);
			AsyncHelpers.RunSync(() => _uintraSearchRepository.IndexAsync(searchableMembers));

			return BroadcastResult.Success;
		}
	}
}