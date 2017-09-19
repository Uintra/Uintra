using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extentions;
using uIntra.Core.Persistence;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Sql;
using uIntra.Users;

namespace Compent.uIntra.Core.Groups
{
    public class GroupMemberService : GroupMemberServiceBase
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;
        private readonly IIntranetUserService<IGroupMember> _intranetUserService;
        private readonly ICacheableIntranetUserService _userCacheService;

        public GroupMemberService(
            ISqlRepository<GroupMember> groupMemberRepository,
            IIntranetUserService<IGroupMember> intranetUserService, 
            ICacheableIntranetUserService userCacheService) : base(groupMemberRepository)
        {
            _groupMemberRepository = groupMemberRepository;
            _intranetUserService = intranetUserService;
            _userCacheService = userCacheService;
        }

        public override void Add(Guid groupId, Guid memberId)
        {
            AddMany(groupId, memberId.ToEnumerableOfOne());
        }

        public override void AddMany(Guid groupId, IEnumerable<Guid> memberIds)
        {
            var groupMembers = new List<GroupMember>();

            foreach (var memberId in memberIds)
            {
                groupMembers.Add(GetNewGroupMember(groupId, memberId));
                var member = _intranetUserService.Get(memberId);
                member.GroupIds = member.GroupIds.Union(groupId.ToEnumerableOfOne());
                _userCacheService.UpdateUserCache(memberId);
            }
            _groupMemberRepository.Add(groupMembers);
        }

        public override void Remove(Guid groupId, Guid memberId)
        {
            _groupMemberRepository.Delete(gm => gm.GroupId == groupId && gm.MemberId == memberId);
            _userCacheService.UpdateUserCache(memberId);
        }
    }
}