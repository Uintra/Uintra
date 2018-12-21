using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Persistence;
using Uintra.Groups;
using Uintra.Groups.Sql;
using Uintra.Users;
using static LanguageExt.Prelude;

namespace Compent.Uintra.Core.Groups
{
    public class GroupMemberService : GroupMemberServiceBase
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;
        private readonly ICacheableIntranetUserService _userCacheService;

        public GroupMemberService(
            ISqlRepository<GroupMember> groupMemberRepository,
            ICacheableIntranetUserService userCacheService) : base(groupMemberRepository)
        {
            _groupMemberRepository = groupMemberRepository;
            _userCacheService = userCacheService;
        }

        public override void Add(Guid groupId, Guid memberId) => 
            AddMany(groupId, List(memberId));

        public override void AddMany(Guid groupId, IEnumerable<Guid> memberIds)
        {
            var enumeratedMemberIds = memberIds as Guid[] ?? memberIds.ToArray();
            var groupMembers = enumeratedMemberIds
                .Select(memberId => GetNewGroupMember(groupId, memberId))
                .ToList();

            _groupMemberRepository.Add(groupMembers);
            _userCacheService.UpdateUserCache(enumeratedMemberIds);
        }

        public override void Remove(Guid groupId, Guid memberId)
        {
            _groupMemberRepository.Delete(gm => gm.GroupId == groupId && gm.MemberId == memberId);
            _userCacheService.UpdateUserCache(memberId);
        }
    }
}