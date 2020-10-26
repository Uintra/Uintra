using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Sql;

namespace Uintra20.Features.Groups.Services
{
    public interface IGroupMemberService
    {
        void Add(Guid groupId, GroupMemberSubscriptionModel subscription);
        Task AddAsync(Guid groupId, GroupMemberSubscriptionModel model);

        void AddMany(Guid groupId, IEnumerable<GroupMemberSubscriptionModel> subscriptions);
        Task AddManyAsync(Guid groupId, IEnumerable<GroupMemberSubscriptionModel> subscriptions);

        void Remove(Guid groupId, Guid memberId);
        Task RemoveAsync(Guid groupId, Guid memberId);

        IEnumerable<GroupMember> GetGroupMemberByMember(Guid memberId);
        Task<IEnumerable<GroupMember>> GetGroupMemberByMemberAsync(Guid memberId);

        IEnumerable<GroupMember> GetManyGroupMember(IEnumerable<Guid> memberIds);
        Task<IEnumerable<GroupMember>> GetManyGroupMemberAsync(IEnumerable<Guid> memberIds);

        int GetMembersCount(Guid groupId);
        Task<int> GetMembersCountAsync(Guid groupId);

        IEnumerable<GroupMember> GetGroupMemberByGroup(Guid groupId);
        Task<IEnumerable<GroupMember>> GetGroupMemberByGroupAsync(Guid groupId);

        bool IsGroupMember(Guid groupId, Guid userId);
        Task<bool> IsGroupMemberAsync(Guid groupId, Guid userId);

        bool IsGroupMember(Guid groupId, IGroupMember member);

        Guid Create(GroupCreateModel model, GroupMemberSubscriptionModel creator);
        Task<Guid> CreateAsync(GroupCreateModel model, GroupMemberSubscriptionModel creator);

        GroupMember GetByMemberId(Guid id);
        Task<GroupMember> GetByMemberIdAsync(Guid id);

        void Update(GroupMember groupMember);
        Task UpdateAsync(GroupMember groupMember);

        GroupMember GetGroupMemberByMemberIdAndGroupId(Guid memberId, Guid groupId);
        Task<GroupMember> GetGroupMemberByMemberIdAndGroupIdAsync(Guid memberId, Guid groupId);

        bool IsMemberAdminOfGroup(Guid memberId, Guid groupId);
        Task<bool> IsMemberAdminOfGroupAsync(Guid memberId, Guid groupId);

        void ToggleAdminRights(Guid memberId, Guid groupId);
        Task ToggleAdminRightsAsync(Guid memberId, Guid groupId);
    }
}