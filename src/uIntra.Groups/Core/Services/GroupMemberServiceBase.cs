using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Persistence;
using uIntra.Groups.Sql;

namespace uIntra.Groups
{
    public abstract class GroupMemberServiceBase : IGroupMemberService
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;

        public GroupMemberServiceBase(ISqlRepository<GroupMember> groupMemberRepository)
        {
            _groupMemberRepository = groupMemberRepository;
        }

        public abstract void Add(Guid groupId, Guid memberId);

        public abstract void AddMany(Guid groupId, IEnumerable<Guid> memberIds);

        protected GroupMember GetNewGroupMember(Guid groupId, Guid memberId)
        {
            return new GroupMember()
            {
                Id = Guid.NewGuid(),
                MemberId = memberId,
                GroupId = groupId
            };
        }

        public virtual void Remove(Guid groupId, Guid memberId)
        {
            _groupMemberRepository.Delete(gm => gm.GroupId == groupId && gm.MemberId == memberId);
        }

        public virtual IEnumerable<GroupMember> GetGroupMemberByMember(Guid memberId)
        {
            return _groupMemberRepository.FindAll(gm => gm.MemberId == memberId);
        }

        public virtual IEnumerable<GroupMember> GetManyGroupMember(IEnumerable<Guid> memberIds)
        {
            return memberIds.Join(_groupMemberRepository.GetAll(), m => m, gm => gm.MemberId, (m, gm) => gm);
        }

        public virtual int GetMembersCount(Guid groupId)
        {
            return (int)_groupMemberRepository.Count(gm => gm.GroupId == groupId);
        }

        public virtual IEnumerable<GroupMember> GetGroupMemberByGroup(Guid groupId)
        {
            return _groupMemberRepository.FindAll(gm => gm.GroupId == groupId);
        }

        public virtual bool IsGroupMember(Guid groupId, Guid userId)
        {
            return _groupMemberRepository.Exists(gm => gm.GroupId == groupId && gm.MemberId == userId);
        }

        public virtual bool IsGroupMember(Guid groupId, IGroupMember member)
        {
            return member.GroupIds.Contains(groupId);
        }
    }
}