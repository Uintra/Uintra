using System.Collections.Generic;
using System.Linq;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.Models;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Extensions;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Features.Permissions.Implementation
{
    public class IntranetMemberGroupService : IIntranetMemberGroupService
    {
        protected virtual string IntranetMemberGroupCacheKey => "IntranetMemberGroupCache";
        private readonly IMemberService _memberService;
        private readonly IMemberGroupService _memberGroupService;
        private readonly ICacheService _cacheService;

        public IntranetMemberGroupService(IMemberGroupService memberGroupService,
            IMemberService memberService,
            ICacheService cacheService)
        {
            _memberGroupService = memberGroupService;
            _memberService = memberService;
            _cacheService = cacheService;
        }

        protected virtual IEnumerable<IntranetMemberGroup> CurrentCache
        {
            get => _cacheService.GetOrSet(IntranetMemberGroupCacheKey,
                () => _memberGroupService
                    .GetAll()
                    .Map<IEnumerable<IntranetMemberGroup>>());
            //get => _mapper.Map<IEnumerable<IntranetMemberGroup>>(_cacheService.GetOrSet(IntranetMemberGroupCacheKey,
            //    () => _memberGroupService.GetAll()));

            set => _cacheService.Set(IntranetMemberGroupCacheKey, value);
        }

        public virtual IEnumerable<IntranetMemberGroup> GetAll() => CurrentCache;

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
            _memberGroupService.Save(new MemberGroup { Name = name });
            group = _memberGroupService.GetByName(name);

            CurrentCache = CurrentCache.Append(group.Map<IntranetMemberGroup>());

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

            CurrentCache = CurrentCache.Where(x => x.Id != id).Append(memberGroup.Map<IntranetMemberGroup>());

            return true;
        }

        public virtual void Delete(int id)
        {
            var group = _memberGroupService.GetById(id);
            _memberGroupService.Delete(group);

            CurrentCache = CurrentCache.Where(x => x.Id != id);
        }

        public void AssignDefaultMemberGroup(int memberId)
        {
            var groups = GetAll();
            var member = groups.First(i => i.Name.Equals("UiUser"));

            if (member != null)
                _memberService.AssignRole(memberId, member.Name);
        }

        public void ClearCache()
        {
            _cacheService.Remove(IntranetMemberGroupCacheKey);
        }

    }
}