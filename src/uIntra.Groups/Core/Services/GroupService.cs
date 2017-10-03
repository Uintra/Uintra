using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Caching;
using uIntra.Core.Persistence;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Groups.Sql;

namespace uIntra.Groups
{
    public class GroupService : IGroupService
    {
        private readonly ISqlRepository<Group> _groupRepository;
        private readonly ICacheService _memoryCacheService;
        private readonly IPermissionsService _permissionsService;
        private const string GroupCacheKey = "Groups";

        public GroupService(
            ISqlRepository<Group> groupRepository,
            ICacheService memoryCacheService,
            IPermissionsService permissionsService)
        {
            _groupRepository = groupRepository;
            _memoryCacheService = memoryCacheService;
            _permissionsService = permissionsService;
        }

        public void Create(GroupModel groupModel)
        {
            var date = DateTime.Now;
            groupModel.CreatedDate = date;
            groupModel.UpdatedDate = date;
            groupModel.Id = Guid.NewGuid();

            _groupRepository.Add(groupModel);
            UpdateCache();
        }

        public void Edit(GroupModel groupModel)
        {
            groupModel.UpdatedDate = DateTime.Now;
            _groupRepository.Update(groupModel);
            UpdateCache();
        }

        public GroupModel Get(Guid id)
        {
            return GetAll().SingleOrDefault(g => g.Id == id);
        }
        
        public IEnumerable<GroupModel> GetAllHided()
        {
            return GetAll().Where(g => g.IsHidden);
        }

        public IEnumerable<GroupModel> GetAll()
        {
            var groups = _memoryCacheService.GetOrSet(GroupCacheKey, () => _groupRepository.GetAll().ToList(), GetCacheExpiration());
            return groups;
        }

        public IEnumerable<GroupModel> GetAllNotHidden()
        {
            return GetAll().Where(g => !g.IsHidden);
        }

        public IEnumerable<GroupModel> GetMany(IEnumerable<Guid> groupIds)
        {
            return GetAllNotHidden().Join(groupIds, g => g.Id, id => id, ((g, id) => g));
        }

        public void UpdateGroupUpdateDate(Guid id)
        {
            var group = Get(id);
            Edit(group);
        }

        public bool CanEdit(Guid groupId, IIntranetUser user)
        {
            var group = Get(groupId);
            return CanEdit(group, user);
        }

        public bool CanEdit(GroupModel groupModel, IIntranetUser user)
        {
            if (_permissionsService.IsUserWebmaster(user))
            {
                return true;
            }

            return groupModel.CreatorId == user.Id;
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