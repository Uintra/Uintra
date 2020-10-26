using System.Collections.Generic;
using System.Linq;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Features.Permissions.Implementation
{
    public class IntranetMemberGroupService : IIntranetMemberGroupService
    {
        protected virtual string IntranetMemberGroupCacheKey => "IntranetMemberGroupCache";
        private readonly ILogger _logger;
        private readonly IMemberService _memberService;
        private readonly IMemberGroupService _memberGroupService;
        private readonly ICacheService _cacheService;

        public IntranetMemberGroupService(IMemberGroupService memberGroupService,
            IMemberService memberService,
            ICacheService cacheService,
            ILogger logger)
        {
            _memberGroupService = memberGroupService;
            _memberService = memberService;
            _cacheService = cacheService;
            _logger = logger;
        }

        protected virtual IEnumerable<IntranetMemberGroup> CurrentCache()
        {
            return _cacheService.GetOrSet(IntranetMemberGroupCacheKey, () => _memberGroupService.GetAll().Map<IEnumerable<IntranetMemberGroup>>());
        }

        public virtual IEnumerable<IntranetMemberGroup> GetAll() => CurrentCache();

        public virtual IEnumerable<IntranetMemberGroup> GetForMember(int id)
        {
            var allGroups = GetAll();
            var groupNamesAssignedToMember = _memberService.GetAllRoles(id);

            var memberGroups = allGroups
                .Join(groupNamesAssignedToMember, group => group.Name, x => x, (group, _) => group).ToList();

            return memberGroups;
        }

        public virtual int Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return int.MinValue;
            var group = _memberGroupService.GetByName(name);
            if (group != null) return group.Id;
            _memberGroupService.Save(new MemberGroup {Name = name});
            group = _memberGroupService.GetByName(name);

            ClearCache();

            return group.Id;
        }

        public virtual bool Save(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            var groupByName = _memberGroupService.GetByName(name);
            if (groupByName != null && groupByName.Id != id) return false;
            var memberGroup = _memberGroupService.GetById(id);
            memberGroup.Name = name;
            _memberGroupService.Save(memberGroup);

            ClearCache();

            return true;
        }

        public virtual void Delete(int id)
        {
            var group = _memberGroupService.GetById(id);
            _memberGroupService.Delete(group);

            ClearCache();
        }

        public void RemoveFromAll(int memberId)
        {
            var memberRolesNames = GetForMember(memberId).Select(r => r.Name);
            _memberService.DissociateRoles(new[] {memberId}, memberRolesNames.ToArray());
        }

        public void AssignDefaultMemberGroup(int memberId)
        {
            var uiUserGroup = GetAll().FirstOrDefault(i => i.Name.Equals("UiUser"));

            if (uiUserGroup != null)
            {
                _memberService.AssignRole(memberId, uiUserGroup.Name);
            }
        }

        public void ClearCache()
        {
            _cacheService.Remove(IntranetMemberGroupCacheKey);
        }
    }
}