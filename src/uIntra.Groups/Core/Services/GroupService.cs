using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using uIntra.Core;
using uIntra.Core.Caching;
using uIntra.Core.Persistence;
using uIntra.Core.User;
using uIntra.Core.User.Permissions;
using uIntra.Groups.Constants;
using uIntra.Groups.Sql;
using Group = uIntra.Groups.Sql.Group;

namespace uIntra.Groups
{
    public class GroupService : IGroupService
    {
        private readonly ISqlRepository<Group> _groupRepository;
        private readonly ISqlRepository<GroupActivity> _groupActivityRepository;
        private readonly ICacheService _memoryCacheService;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IPermissionsService _permissionsService;
        private const string GroupCacheKey = "Groups";

        public GroupService(
            ISqlRepository<Group> groupRepository,
            ICacheService memoryCacheService,
            IGroupMemberService groupMemberService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IPermissionsService permissionsService,
            ISqlRepository<GroupActivity> groupActivityRepository)
        {
            _groupRepository = groupRepository;
            _memoryCacheService = memoryCacheService;
            _groupMemberService = groupMemberService;
            _intranetUserService = intranetUserService;
            _permissionsService = permissionsService;
            _groupActivityRepository = groupActivityRepository;
        }

        public void Create(Group group)
        {
            var date = DateTime.Now;
            group.CreatedDate = date;
            group.UpdatedDate = date;
            group.Id = Guid.NewGuid();

            _groupRepository.Add(group);
            FillCache(group);
        }

        public void Edit(Group group)
        {
            group.UpdatedDate = DateTime.Now;
            _groupRepository.Update(group);
            FillCache(group);
        }

        public Group Get(Guid id)
        {
            return GetAll().SingleOrDefault(g => g.Id == id);
        }
        
        public IEnumerable<Group> GetAllHided()
        {
            return GetAll().Where(g => g.IsHidden);
        }

        public IEnumerable<Group> GetAll()
        {
            var groups = _memoryCacheService.GetOrSet(GroupCacheKey, () => _groupRepository.GetAll().ToList(), GetCacheExpiration());
            return groups;
        }

        public IEnumerable<Group> GetAllNotHidden()
        {
            return GetAll().Where(g => !g.IsHidden);
        }

        public IEnumerable<Group> GetMany(IEnumerable<Guid> groupIds)
        {
            return GetAllNotHidden().Join(groupIds, g => g.Id, id => id, ((g, id) => g));
        }

        public void FillGroupActivityData(IGroupActivity activity, bool isGroupPage)
        {
            if (activity.GroupId.HasValue)
            {
                var group = Get(activity.GroupId.Value);
                if (!isGroupPage)
                {
                    if (!group.IsHidden)
                    {
                        activity.HeaderInfo.GroupId = group.Id;
                        activity.HeaderInfo.GroupTitle = group.Title;
                    }
                }

                var currentUserId = _intranetUserService.GetCurrentUserId();
                var isGroupMember = _groupMemberService.IsGroupMember(group.Id, currentUserId);
                activity.IsReadonly = !isGroupMember;
            }
            else
            {
                activity.IsReadonly = false;
            }
        }

        public bool CanCreate(IHaveCreator activity, IIntranetUser user)
        {
            return _permissionsService.IsUserWebmaster(user) || activity?.CreatorId == user.Id;
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

        public bool CanEdit(Group group, IIntranetUser user)
        {
            if (_permissionsService.IsUserWebmaster(user))
            {
                return true;
            }

            return group.CreatorId == user.Id;
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

        public void AddGroupActivityRelation(Guid groupId, Guid activityId)
        {
            var relation = new GroupActivity()
            {
                ActivityId = activityId,
                GroupId = groupId,
                Id = Guid.NewGuid()
            };

            _groupActivityRepository.Add(relation);
        }

        public void RemoveGroupActivityRelation(Guid groupId, Guid activityId)
        {
            _groupActivityRepository.Delete(r => r.ActivityId.Equals(activityId) && r.GroupId.Equals(groupId));
        }

        public IEnumerable<Guid> GetGroupIds(Guid activityId)
        {
            return _groupActivityRepository
                .FindAll(rel => rel.ActivityId == activityId)
                .Select(rel => rel.GroupId);
        }


        private static DateTimeOffset GetCacheExpiration()
        {
            return DateTimeOffset.Now.AddDays(1);
        }

        private void FillCache(Group group)
        {
            var groups = GetAll().ToList();
            groups = groups.FindAll(a => a.Id != group.Id);
            groups.Add(group);
            _memoryCacheService.Set(GroupCacheKey, groups, GetCacheExpiration());
        }

        public Guid? GetGroupIdFromQuery(string query)
        {
            var regEx = GroupConstants.GroupIdQueryParam + @"=([0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12})";
            var activityIdMatch = Regex.Match(query, regEx, RegexOptions.IgnoreCase);

            if (activityIdMatch.Success)
            {
                return new Guid(activityIdMatch.Groups[1].Value);
            }
            return null;
        }
    }
}