using System;
using System.Collections.Generic;
using uIntra.Groups.Sql;

namespace uIntra.Groups
{
    public interface IGroupMemberService
    {
        void Add(Guid groupId, Guid memberId);

        void AddMany(Guid groupId, IEnumerable<Guid> memberId);

        void Remove(Guid groupId, Guid memberId);

        IEnumerable<GroupMember> GetGroupMemberByMember(Guid memberId);

        IEnumerable<GroupMember> GetManyGroupMember(IEnumerable<Guid> memberIds);

        int GetMembersCount(Guid groupId);

        IEnumerable<GroupMember> GetGroupMemberByGroup(Guid groupId);

        bool IsGroupMember(Guid groupId, Guid userId);

        bool IsGroupMember(Guid groupId, IGroupMember member);
    }
}