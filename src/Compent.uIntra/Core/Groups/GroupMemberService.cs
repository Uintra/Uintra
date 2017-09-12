using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extentions;
using uIntra.Core.Persistence;
using uIntra.Core.User;
using uIntra.Groups;
using uIntra.Groups.Sql;

namespace Compent.uIntra.Core.Groups
{
    public class GroupMemberService : GroupMemberServiceBase
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;
        private readonly IIntranetUserService<IGroupMember> _intranetUserService;

        public GroupMemberService(
            ISqlRepository<GroupMember> groupMemberRepository,
            IIntranetUserService<IGroupMember> intranetUserService) : base(groupMemberRepository)
        {
            _groupMemberRepository = groupMemberRepository;
            _intranetUserService = intranetUserService;
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
            }

            _groupMemberRepository.Add(groupMembers);
        }
    }
}