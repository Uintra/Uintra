using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Uintra.Core.Persistence;
using Uintra.Groups.Sql;

namespace Uintra.Groups
{
    public abstract class GroupMemberServiceBase : IGroupMemberService
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;

        protected GroupMemberServiceBase(ISqlRepository<GroupMember> groupMemberRepository)
        {
            _groupMemberRepository = groupMemberRepository;
        }

        public abstract void Add(Guid groupId, Guid memberId);

        public abstract void AddMany(Guid groupId, IEnumerable<Guid> memberIds);

        protected GroupMember GetNewGroupMember(Guid groupId, Guid memberId)
        {
            return new GroupMember
            {
                Id = Guid.NewGuid(),
                MemberId = memberId,
                GroupId = groupId,
                IsAdmin = true
            };
        }

        public abstract void Remove(Guid groupId, Guid memberId);

        public virtual IEnumerable<GroupMember> GetGroupMemberByMember(Guid memberId)
        {
            var members = _groupMemberRepository.FindAll(gm => gm.MemberId == memberId);
            var mappedMembers = Mapper.Map<IEnumerable<GroupMember>>(members);
            return mappedMembers;
        }

        public virtual IEnumerable<GroupMember> GetManyGroupMember(IEnumerable<Guid> memberIds)
        {
            var members = memberIds.Join(_groupMemberRepository.GetAll(), m => m, gm => gm.MemberId, (m, gm) => gm);
            var mappedMembers = Mapper.Map<IEnumerable<GroupMember>>(members);
            return mappedMembers;
        }

        public virtual int GetMembersCount(Guid groupId)
        {
            return (int)_groupMemberRepository.Count(gm => gm.GroupId == groupId);
        }

        public virtual IEnumerable<GroupMember> GetGroupMemberByGroup(Guid groupId)
        {
            var members = _groupMemberRepository.FindAll(gm => gm.GroupId == groupId);
            var mappedMembers = Mapper.Map<IEnumerable<GroupMember>>(members);
            return mappedMembers;
        }

        public virtual bool IsGroupMember(Guid groupId, Guid userId)
        {
            return _groupMemberRepository.Exists(gm => gm.GroupId == groupId && gm.MemberId == userId);
        }

        public virtual bool IsGroupMember(Guid groupId, IGroupMember member)
        {
            return member.GroupIds.Contains(groupId);
        }

        public abstract string Create(GroupCreateModel model);
    }
}