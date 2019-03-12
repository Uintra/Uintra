using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Interfaces;
using Uintra.Core.Permissions.Models;
using Uintra.Core.Persistence;
using Uintra.Core.User;
using Uintra.Groups.Permissions;
using Uintra.Groups.Sql;
using Umbraco.Core.Models;
using static LanguageExt.Prelude;

namespace Uintra.Groups
{
    public class GroupService : IGroupService
    {
        private const string GroupCacheKey = "Groups";
        private readonly ISqlRepository<Group> _groupRepository;
        private readonly ICacheService _memoryCacheService;
        private readonly IPermissionsService _permissionsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;

        protected const string GroupsCreatePage = "groupsCreatePage";
        protected Enum PermissionResourceType => PermissionResourceTypeEnum.Groups;

        public GroupService(
            ISqlRepository<Group> groupRepository,
            ICacheService memoryCacheService,
            IPermissionsService permissionsService,
            IIntranetMemberService<IIntranetMember> intranetMemberService)
        {
            _groupRepository = groupRepository;
            _memoryCacheService = memoryCacheService;
            _permissionsService = permissionsService;
            _intranetMemberService = intranetMemberService;
        }

        public Guid Create(GroupModel model)
        {
            var date = DateTime.Now;
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
            var date = DateTime.Now;
            var group = model.Map<Group>();
            group.UpdatedDate = date;
            _groupRepository.Update(group);
            UpdateCache();
        }

        public GroupModel Get(Guid id)
        {
            return GetAll().SingleOrDefault(g => g.Id == id);
        }

        public IEnumerable<GroupModel> GetAll()
        {
            var groups = _memoryCacheService.GetOrSet(GroupCacheKey, () => _groupRepository.GetAll().ToList(), GetCacheExpiration());
            return groups.Map<IEnumerable<GroupModel>>();
        }

        public IEnumerable<GroupModel> GetAllNotHidden()
        {
            return GetAll().Where(g => !g.IsHidden);
        }

        public IEnumerable<GroupModel> GetMany(IEnumerable<Guid> groupIds)
        {
            return GetAllNotHidden().Join(groupIds, g => g.Id, identity, (g, _) => g);
        }

        public bool CanHide(Guid id)
        {
            var group = Get(id);
            return CanHide(group);
        }

        public bool CanHide(GroupModel group)
        {
            return CanPerform(group, PermissionActionEnum.Hide, PermissionActionEnum.HideOther);
        }

        public bool CanEdit(Guid id)
        {
            var group = Get(id);
            return CanEdit(group);
        }

        public bool CanEdit(GroupModel group)
        {
            return CanPerform(group, PermissionActionEnum.Edit, PermissionActionEnum.EditOther);
        }

        public bool CanPerform(GroupModel group, Enum action, Enum administrationAction)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();
            var ownerId = group.CreatorId;
            var isOwner = ownerId == currentMember.Id;

            var act = isOwner ? action : administrationAction;
            var result = _permissionsService.Check(currentMember, PermissionSettingIdentity.Of(act, PermissionResourceType));

            return result;
        }

        public bool ValidatePermission(IPublishedContent content)
        {
            if (content.DocumentTypeAlias == GroupsCreatePage)
            {
                var hasPermission = _permissionsService.Check(PermissionSettingIdentity.Of(PermissionActionEnum.Create, PermissionResourceType));
                return hasPermission;
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

        public bool IsActivityFromActiveGroup(IGroupActivity groupActivity) =>
            groupActivity.GroupId.HasValue && !Get(groupActivity.GroupId.Value).IsHidden;

        private static DateTimeOffset GetCacheExpiration()
        {
            return DateTimeOffset.Now.AddDays(1);
        }

        private void UpdateCache()
        {
            var groups = _groupRepository.GetAll().ToList();
            _memoryCacheService.Set(GroupCacheKey, groups, GetCacheExpiration());
        }
    }
}