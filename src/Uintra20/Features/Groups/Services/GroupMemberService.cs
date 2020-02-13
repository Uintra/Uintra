using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Sql;
using Uintra20.Features.Media;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Groups.Services
{
    public class GroupMemberService : IGroupMemberService
    {
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;
        private readonly IIntranetMemberService<IntranetMember> _memberService;
        private readonly ICacheableIntranetMemberService _memberCacheService;
        private readonly IMediaHelper _mediaHelper;
        private readonly IGroupService _groupService;
        private readonly IGroupMediaService _groupMediaService;

        public GroupMemberService(
            ISqlRepository<GroupMember> groupMemberRepository,
            ICacheableIntranetMemberService memberCacheService,
            IMediaHelper mediaHelper,
            IGroupService groupService,
            IGroupMediaService groupMediaService,
            IIntranetMemberService<IntranetMember> memberService)
        {
            _groupMemberRepository = groupMemberRepository;
            _memberCacheService = memberCacheService;
            _mediaHelper = mediaHelper;
            _groupService = groupService;
            _groupMediaService = groupMediaService;
            _memberService = memberService;
        }

        public void Add(Guid groupId, GroupMemberSubscriptionModel model)
        {
            var groupMember = GetNewGroupMember(groupId, model);
            _groupMemberRepository.Add(groupMember);
            _memberCacheService.UpdateMemberCache(groupMember.MemberId);
        }

        public async Task AddAsync(Guid groupId, GroupMemberSubscriptionModel model)
        {
            var groupMember = GetNewGroupMember(groupId, model);
            await _groupMemberRepository.AddAsync(groupMember);
            await _memberCacheService.UpdateMemberCacheAsync(groupMember.MemberId);
        }

        public void AddMany(Guid groupId, IEnumerable<GroupMemberSubscriptionModel> subscriptions)
        {
            var groupMembers = subscriptions
                .Select(memberId => GetNewGroupMember(groupId, memberId))
                .ToList();

            _groupMemberRepository.Add(groupMembers);
            _memberCacheService.UpdateMemberCache(groupMembers.Select(g => g.MemberId));
        }

        public async Task AddManyAsync(Guid groupId, IEnumerable<GroupMemberSubscriptionModel> subscriptions)
        {
            var groupMembers = subscriptions
                .Select(memberId => GetNewGroupMember(groupId, memberId))
                .ToList();

            await _groupMemberRepository.AddAsync(groupMembers);
            await _memberCacheService.UpdateMemberCacheAsync(groupMembers.Select(g => g.MemberId));
        }

        public void Remove(Guid groupId, Guid memberId)
        {
            _groupMemberRepository.Delete(IsGroupAndUserMatch(memberId, groupId));
            _memberCacheService.UpdateMemberCache(memberId);
        }

        public async Task RemoveAsync(Guid groupId, Guid memberId)
        {
            await _groupMemberRepository.DeleteAsync(IsGroupAndUserMatch(memberId, groupId));
            await _memberCacheService.UpdateMemberCacheAsync(memberId);
        }

        public IEnumerable<GroupMember> GetGroupMemberByMember(Guid memberId)
        {
            var members = _groupMemberRepository.FindAll(gm => gm.MemberId == memberId);
            var mappedMembers = members.Map<IEnumerable<GroupMember>>();
            return mappedMembers;
        }

        public async Task<IEnumerable<GroupMember>> GetGroupMemberByMemberAsync(Guid memberId)
        {
            var members = await _groupMemberRepository.FindAllAsync(gm => gm.MemberId == memberId);
            var mappedMembers = members.Map<IEnumerable<GroupMember>>();
            return mappedMembers;
        }

        public IEnumerable<GroupMember> GetManyGroupMember(IEnumerable<Guid> memberIds)
        {
            var members = memberIds.Join(_groupMemberRepository.GetAll(), m => m, gm => gm.MemberId, (m, gm) => gm);
            var mappedMembers = members.Map<IEnumerable<GroupMember>>();
            return mappedMembers;
        }

        public async Task<IEnumerable<GroupMember>> GetManyGroupMemberAsync(IEnumerable<Guid> memberIds)
        {
            var members = memberIds.Join(await _groupMemberRepository.GetAllAsync(), m => m, gm => gm.MemberId, (m, gm) => gm);
            var mappedMembers = members.Map<IEnumerable<GroupMember>>();
            return mappedMembers;
        }

        public int GetMembersCount(Guid groupId)
        {
            return (int)_groupMemberRepository.Count(gm => gm.GroupId == groupId);
        }

        public async Task<int> GetMembersCountAsync(Guid groupId)
        {
            return (int)await _groupMemberRepository.CountAsync(gm => gm.GroupId == groupId);
        }

        public IEnumerable<GroupMember> GetGroupMemberByGroup(Guid groupId)
        {
            var members = _groupMemberRepository.FindAll(gm => gm.GroupId == groupId);
            var mappedMembers = members.Map<IEnumerable<GroupMember>>();
            return mappedMembers;
        }

        public async Task<IEnumerable<GroupMember>> GetGroupMemberByGroupAsync(Guid groupId)
        {
            var members = await _groupMemberRepository.FindAllAsync(gm => gm.GroupId == groupId);
            var mappedMembers = members.Map<IEnumerable<GroupMember>>();
            return mappedMembers;
        }

        public bool IsGroupMember(Guid groupId, Guid userId)
        {
            return _groupMemberRepository.Exists(gm => gm.GroupId == groupId && gm.MemberId == userId);
        }

        public async Task<bool> IsGroupMemberAsync(Guid groupId, Guid userId)
        {
            return await _groupMemberRepository.ExistsAsync(gm => gm.GroupId == groupId && gm.MemberId == userId);
        }

        public bool IsGroupMember(Guid groupId, IGroupMember member)
        {
            return member.GroupIds.Contains(groupId);
        }

        public Guid Create(GroupCreateModel model, GroupMemberSubscriptionModel creator)
        {
            var group = model.Map<GroupModel>();
            group.CreatorId = _memberService.GetCurrentMemberId();

            group.GroupTypeId = GroupTypeEnum.Open.ToInt();

            var createdMedias = _mediaHelper.CreateMedia(model, MediaFolderTypeEnum.GroupsContent).ToList();

            group.ImageId = createdMedias.Any()
                ? (int?)createdMedias.First()
                : null;

            var groupId = _groupService.Create(group);

            Add(groupId, creator);

            _groupMediaService.GroupTitleChanged(groupId, @group.Title);

            return groupId;
        }

        public async Task<Guid> CreateAsync(GroupCreateModel model, GroupMemberSubscriptionModel creator)
        {
            var group = model.Map<GroupModel>();
            group.CreatorId = await _memberService.GetCurrentMemberIdAsync();

            group.GroupTypeId = GroupTypeEnum.Open.ToInt();

            var createdMedias = _mediaHelper.CreateMedia(model, MediaFolderTypeEnum.GroupsContent).ToList();

            group.ImageId = createdMedias.Any()
                ? (int?)createdMedias.First()
                : null;

            var groupId = await _groupService.CreateAsync(group);
            await AddAsync(groupId, creator);

            await _groupMediaService.GroupTitleChangedAsync(groupId, @group.Title);

            return groupId;
        }

        public GroupMember GetByMemberId(Guid id) =>
            _groupMemberRepository.Find(gm => gm.MemberId == id);

        public Task<GroupMember> GetByMemberIdAsync(Guid id) =>
            _groupMemberRepository.FindAsync(gm => gm.MemberId == id);

        public void Update(GroupMember groupMember)
        {
            _groupMemberRepository.Update(groupMember);
            _memberCacheService.UpdateMemberCache(groupMember.MemberId);
        }

        public async Task UpdateAsync(GroupMember groupMember)
        {
            await _groupMemberRepository.UpdateAsync(groupMember);
            await _memberCacheService.UpdateMemberCacheAsync(groupMember.MemberId);
        }

        public GroupMember GetGroupMemberByMemberIdAndGroupId(
            Guid memberId,
            Guid groupId) =>
            _groupMemberRepository.Find(IsGroupAndUserMatch(memberId, groupId));

        public Task<GroupMember> GetGroupMemberByMemberIdAndGroupIdAsync(
            Guid memberId,
            Guid groupId) =>
            _groupMemberRepository.FindAsync(IsGroupAndUserMatch(memberId, groupId));

        public bool IsMemberAdminOfGroup(Guid memberId, Guid groupId) =>
            GetGroupMemberByMemberIdAndGroupId(memberId, groupId)?.IsAdmin ?? false;

        public async Task<bool> IsMemberAdminOfGroupAsync(Guid memberId, Guid groupId) =>
            (await GetGroupMemberByMemberIdAndGroupIdAsync(memberId, groupId))?.IsAdmin ?? false;

        public void ToggleAdminRights(Guid memberId, Guid groupId)
        {
            var groupMember = GetGroupMemberByMemberIdAndGroupId(memberId, groupId);

            groupMember.IsAdmin = !groupMember.IsAdmin;

            Update(groupMember);
            _memberCacheService.UpdateMemberCache(memberId);
        }

        public async Task ToggleAdminRightsAsync(Guid memberId, Guid groupId)
        {
            var groupMember = await GetGroupMemberByMemberIdAndGroupIdAsync(memberId, groupId);

            groupMember.IsAdmin = !groupMember.IsAdmin;

            await UpdateAsync(groupMember);
            await _memberCacheService.UpdateMemberCacheAsync(memberId);
        }

        private GroupMember GetNewGroupMember(Guid groupId, GroupMemberSubscriptionModel subscription)
        {
            return new GroupMember
            {
                Id = Guid.NewGuid(),
                MemberId = subscription.MemberId,
                GroupId = groupId,
                IsAdmin = subscription.IsAdmin
            };
        }

        private static Expression<Func<GroupMember, bool>> IsGroupAndUserMatch(Guid memberId, Guid groupId) =>
            g => g.GroupId == groupId && g.MemberId == memberId;
    }
}