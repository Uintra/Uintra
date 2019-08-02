using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Caching;
using Uintra.Core.Extensions;
using Uintra.Core.Permissions.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using static LanguageExt.Prelude;

namespace Uintra.Core.Permissions.Implementation
{
    public class IntranetMemberGroupService : IIntranetMemberGroupService
    {
        protected virtual string IntranetMemberGroupCacheKey => "IntranetMemberGroupCache";
        private readonly IMemberService _memberService;
        private readonly IMemberGroupService _memberGroupService;
        private readonly ICacheService _cacheService;

        public IntranetMemberGroupService(
            IMemberGroupService memberGroupService,
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

            set => _cacheService.Set(IntranetMemberGroupCacheKey, value);
        }

        public virtual IEnumerable<IntranetMemberGroup> GetAll() =>
            CurrentCache;

        public virtual IEnumerable<IntranetMemberGroup> GetForMember(int id) =>
            GetAll()
                .Join(_memberService.GetAllRoles(id),group => @group.Name, identity,(group, _) => @group)
                .ToList();

        public virtual int Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return int.MinValue;

            var group = _memberGroupService.GetByName(name);

            if (group != null) return group.Id;

            _memberGroupService.Save(new MemberGroup {Name = name});
            group = _memberGroupService.GetByName(name);

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

            return true;
        }

        public virtual void Delete(int id) => 
            _memberGroupService.Delete(_memberGroupService.GetById(id));

        public void AssignDefaultMemberGroup(int memberId) =>
            GetAll()
                .Find(i => i.Name.Equals("UiUser"))
                .IfSome(j => _memberService.AssignRole(memberId, j.Name));

        public void ClearCache() =>
            _cacheService.Remove(IntranetMemberGroupCacheKey);
    }
}