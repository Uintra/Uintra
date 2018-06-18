using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using uIntra.Core.Persistence;
using uIntra.Groups;
using uIntra.Groups.Sql;
using uIntra.Users;

namespace Compent.uIntra.Core.Groups
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

        public override void Add(Guid groupId, Guid memberId)
        {
            AddMany(groupId, memberId.ToEnumerable());
        }

        public override void AddMany(Guid groupId, IEnumerable<Guid> memberIds)
        {
            var groupMembers = new List<GroupMember>();

            var enumeratedMemberIds = memberIds as Guid[] ?? memberIds.ToArray();
            foreach (var memberId in enumeratedMemberIds)
                groupMembers.Add(GetNewGroupMember(groupId, memberId));

            _groupMemberRepository.Add(groupMembers);

            foreach (var memberId in enumeratedMemberIds)
                _userCacheService.UpdateUserCache(memberId); // TODO: ask about extending ICacheableIntranetUserService to re-cache bunch of users
        }

        public override void Remove(Guid groupId, Guid memberId)
        {
            _groupMemberRepository.Delete(gm => gm.GroupId == groupId && gm.MemberId == memberId);
            _userCacheService.UpdateUserCache(memberId);
        }
    }
}