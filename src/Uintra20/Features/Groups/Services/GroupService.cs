using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Models;
using Uintra20.Features.Groups.Sql;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.Groups.Services
{
    public class GroupService : IGroupService
    {
        private const string GroupCacheKey = "Groups";
        private readonly ISqlRepository<Group> _groupRepository;
        private readonly ICacheService _memoryCacheService;
        private readonly IPermissionsService _permissionsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly ISqlRepository<GroupMember> _groupMemberRepository;

        protected const string GroupsCreatePage = "groupsCreatePage";
        protected virtual Enum PermissionResourceType => PermissionResourceTypeEnum.Groups;

        public GroupService(
            ISqlRepository<Group> groupRepository,
            ICacheService memoryCacheService,
            IPermissionsService permissionsService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            ISqlRepository<GroupMember> groupMemberRepository)
        {
            _groupRepository = groupRepository;
            _memoryCacheService = memoryCacheService;
            _permissionsService = permissionsService;
            _intranetMemberService = intranetMemberService;
            _groupMemberRepository = groupMemberRepository;
        }

        #region async

        public async Task<Guid> CreateAsync(GroupModel groupModel)
        {
            var date = DateTime.UtcNow;
            var group = groupModel.Map<Group>();
            group.CreatedDate = date;
            group.UpdatedDate = date;
            group.Id = Guid.NewGuid();

            await _groupRepository.AddAsync(group);
            await UpdateCacheAsync();

            return group.Id;
        }

        public async Task EditAsync(GroupModel groupModel)
        {
            var date = DateTime.UtcNow;
            var group = groupModel.Map<Group>();
            group.UpdatedDate = date;
            await _groupRepository.UpdateAsync(group);
            await UpdateCacheAsync();
        }

        public async Task<GroupModel> GetAsync(Guid id) =>
            (await GetAllAsync()).SingleOrDefault(g => g.Id == id);


        public async Task<IEnumerable<GroupModel>> GetAllNotHiddenAsync() =>
            (await GetAllAsync()).Where(g => !g.IsHidden);

        public async Task<IEnumerable<GroupModel>> GetManyAsync(IEnumerable<Guid> groupIds) =>
            (await GetAllNotHiddenAsync()).Join(groupIds, g => g.Id, x => x, (g, _) => g);

        public async Task<IEnumerable<GroupModel>> GetAllAsync()
        {
            var groups = await _memoryCacheService.GetOrSetAsync(async () => (await _groupRepository.GetAllAsync()).ToList(), GroupCacheKey, GetCacheExpiration());

            return groups.Map<IEnumerable<GroupModel>>();
        }

        public async Task<bool> CanHideAsync(Guid id) =>
            await CanHideAsync(await GetAsync(id));

        public async Task<bool> CanHideAsync(GroupModel @group) =>
            await CanPerformAsync(group);

        public async Task<bool> CanEditAsync(Guid id) =>
            await CanEditAsync(await GetAsync(id));

        public async Task<bool> CanEditAsync(GroupModel @group) =>
            await CanPerformAsync(group);

        public async Task<bool> ValidatePermissionAsync(IPublishedContent content)
        {
            if (content.ContentType.Alias == GroupsCreatePage)
            {
                return await _permissionsService.CheckAsync(PermissionSettingIdentity.Of(PermissionActionEnum.Create, PermissionResourceType));
            }

            return true;
        }

        public async Task<bool> IsActivityFromActiveGroupAsync(IGroupActivity groupActivity) =>
            groupActivity.GroupId.HasValue && !(await GetAsync(groupActivity.GroupId.Value)).IsHidden;


        public async Task<bool> IsMemberCreatorAsync(Guid memberId, Guid groupId) =>
            (await _groupRepository.GetAsync(groupId))?.CreatorId.CompareTo(memberId) == 0;

        public async Task HideAsync(Guid id)
        {
            var group = await GetAsync(id);

            group.IsHidden = true;

            await EditAsync(group);
        }

        public async Task UnhideAsync(Guid id)
        {
            var group = await GetAsync(id);

            group.IsHidden = false;

            await EditAsync(group);
        }

        public async Task<bool> CanPerformAsync(GroupModel group)
        {
            //var currentMember = await _intranetMemberService.GetCurrentMemberAsync();
            var currentMember = _intranetMemberService.GetCurrentMember();

            var isOwner = group.CreatorId == currentMember.Id;

            var groupMember = await _groupMemberRepository.FindAsync(m => m.GroupId == group.Id && m.MemberId == currentMember.Id);

            if (groupMember == null) return false;

            var act = isOwner || groupMember.IsAdmin;

            return act;
        }

        private async Task UpdateCacheAsync() =>
            await _memoryCacheService.SetAsync(
                async () => (await _groupRepository.GetAllAsync()).ToList(),
                GroupCacheKey, 
                GetCacheExpiration());

        #endregion

        #region sunc

        public Guid Create(GroupModel model)
        {
            var date = DateTime.UtcNow;
            var group = model.Map<Group>();
            group.CreatedDate = date;
            group.UpdatedDate = date;
            group.Id = Guid.NewGuid();

            _groupRepository.Add(group);
            UpdateCache();

            return group.Id;
        }

        public void Edit(GroupModel model)
        {
            var date = DateTime.UtcNow;
            var group = model.Map<Group>();
            group.UpdatedDate = date;
            _groupRepository.Update(group);
            UpdateCache();
        }

        public GroupModel Get(Guid id) =>
            GetAll().SingleOrDefault(g => g.Id == id);

        public IEnumerable<GroupModel> GetAll()
        {
            var groups = _memoryCacheService.GetOrSet(GroupCacheKey, () => _groupRepository.GetAll().ToList(), GetCacheExpiration());

            return groups.Map<IEnumerable<GroupModel>>();
        }

        public IEnumerable<GroupModel> GetAllNotHidden() =>
            GetAll().Where(g => !g.IsHidden);

        public IEnumerable<GroupModel> GetMany(IEnumerable<Guid> groupIds) =>
            GetAllNotHidden().Join(groupIds, g => g.Id, x => x, (g, _) => g);

        public bool CanHide(Guid id) =>
            CanHide(Get(id));

        public bool CanHide(GroupModel group) =>
            CanPerform(group);

        public bool CanEdit(Guid id) =>
            CanEdit(Get(id));

        public bool CanEdit(GroupModel group) =>
            CanPerform(group);

        public bool CanPerform(GroupModel group)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            var isOwner = group.CreatorId == currentMember.Id;

            var groupMember = _groupMemberRepository.Find(m => m.GroupId == group.Id && m.MemberId == currentMember.Id);

            if (groupMember == null) return false;

            var act = isOwner || groupMember.IsAdmin;

            return act;
        }

        public bool ValidatePermission(IPublishedContent content)
        {
            if (content.ContentType.Alias == GroupsCreatePage)
            {
                return _permissionsService.Check(PermissionSettingIdentity.Of(PermissionActionEnum.Create, PermissionResourceType));
            }

            return true;
        }

        public void Hide(Guid id)
        {
            var group = Get(id);

            group.IsHidden = true;

            Edit(group);
        }

        public void Unhide(Guid id)
        {
            var group = Get(id);

            group.IsHidden = false;

            Edit(group);
        }

        public bool IsMemberCreator(Guid memberId, Guid groupId) =>
            _groupRepository.Get(groupId)?.CreatorId.CompareTo(memberId) == 0;

        public bool IsActivityFromActiveGroup(IGroupActivity groupActivity) =>
            groupActivity.GroupId.HasValue && !Get(groupActivity.GroupId.Value).IsHidden;

        private static DateTimeOffset GetCacheExpiration() =>
            DateTimeOffset.Now.AddDays(1);

        private void UpdateCache() =>
            _memoryCacheService.Set(GroupCacheKey, _groupRepository.GetAll().ToList(), GetCacheExpiration());

        #endregion
    }
}