using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Persistence;
using uIntra.Groups.Sql;

namespace uIntra.Groups
{
    public class GroupMemberService : IGroupMemberService
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;

        public GroupMemberService(ISqlRepository<GroupMember> groupMemberRepository)
        {
            _groupMemberRepository = groupMemberRepository;
        }

        public void Add(Guid groupId, Guid memberId)
        {
            var groupMember = GetNewGroupMember(groupId, memberId);
            _groupMemberRepository.Add(groupMember);
        }

        public void AddMany(Guid groupId, IEnumerable<Guid> memberIds)
        {
            var groupMembers = memberIds.Select(m => GetNewGroupMember(groupId, m));
            _groupMemberRepository.Add(groupMembers);
        }

        private GroupMember GetNewGroupMember(Guid groupId, Guid memberId)
        {
            return new GroupMember()
            {
                Id = Guid.NewGuid(),
                MemberId = memberId,
                GroupId = groupId
            };
        }

        public void Remove(Guid groupId, Guid memberId)
        {
            _groupMemberRepository.Delete(gm => gm.GroupId == groupId && gm.MemberId == memberId);
        }

        public IEnumerable<GroupMember> GetGroupMemberByMember(Guid memberId)
        {
            return _groupMemberRepository.FindAll(gm => gm.MemberId == memberId);
        }

        public IEnumerable<GroupMember> GetManyGroupMember(IEnumerable<Guid> memberIds)
        {
            return memberIds.Join(_groupMemberRepository.GetAll(), m => m, gm => gm.MemberId, (m, gm) => gm);
        }

        public int GetMembersCount(Guid groupId)
        {
            return (int)_groupMemberRepository.Count(gm => gm.GroupId == groupId);
        }

        public IEnumerable<GroupMember> GetGroupMemberByGroup(Guid groupId)
        {
            return _groupMemberRepository.FindAll(gm => gm.GroupId == groupId);
        }

        public bool IsGroupMember(Guid groupId, Guid userId)
        {
            return _groupMemberRepository.Exists(gm => gm.GroupId == groupId && gm.MemberId == userId);
        }

        public bool IsGroupMember(Guid groupId, IGroupMember member)
        {
            return member.GroupIds.Contains(groupId);
        }
    }
}