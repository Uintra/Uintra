using System;
using System.Collections.Generic;
using Uintra.Groups.Sql;

namespace Uintra.Groups
{
    public interface IGroupMemberService
    {
        void Add(Guid groupId, GroupMemberSubscriptionModel subscription);

        void AddMany(Guid groupId, IEnumerable<GroupMemberSubscriptionModel> subscriptions);

        void Remove(Guid groupId, Guid memberId);

        IEnumerable<GroupMember> GetGroupMemberByMember(Guid memberId);

        IEnumerable<GroupMember> GetManyGroupMember(IEnumerable<Guid> memberIds);

        int GetMembersCount(Guid groupId);

        IEnumerable<GroupMember> GetGroupMemberByGroup(Guid groupId);

        bool IsGroupMember(Guid groupId, Guid userId);

        bool IsGroupMember(Guid groupId, IGroupMember member);

        string Create(GroupCreateModel model);

        GroupMember Get(Guid id);

        void Update(GroupMember groupMember);

        GroupMember GetGroupMemberByMemberIdAndGroupId(Guid memberId, Guid groupId);

        bool IsMemberAdminOfGroup(Guid memberId, Guid groupId);

        void ToggleAdminRights(Guid memberId, Guid groupId);
    }
}