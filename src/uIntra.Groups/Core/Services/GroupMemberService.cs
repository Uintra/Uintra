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
            var groupMember = new GroupMember()
            {
                Id = Guid.NewGuid(),
                GroupId = groupId,
                MemberId = memberId,
            };

            _groupMemberRepository.Add(groupMember);
        }

        public void AddMany(Guid groupId, IEnumerable<Guid> memberId)
        {
            var groupMembers = memberId.Select(m => new GroupMember()
            {
                Id = Guid.NewGuid(),
                MemberId = m,
                GroupId = groupId
            });

            _groupMemberRepository.Add(groupMembers);
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

        public void FillGroupMember(IGroupMember member)
        {
            member.GroupIds = GetGroupMemberByMember(member.Id).Select(g => g.GroupId);
        }
    }
}