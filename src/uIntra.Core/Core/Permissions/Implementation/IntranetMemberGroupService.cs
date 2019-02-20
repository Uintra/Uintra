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
        protected virtual string IntranetMemberGroupCahceKey => "IntranetMemberGroupCache";
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

        protected IEnumerable<IntranetMemberGroup> CurrentCache
        {
            get
            {
                return _cacheService.GetOrSet(IntranetMemberGroupCahceKey,
                    () => _memberGroupService
                        .GetAll()
                        .Map<IEnumerable<IntranetMemberGroup>>());
            }
            set
            {
                _cacheService.Set(IntranetMemberGroupCahceKey, value);
            }
        }

        public IEnumerable<IntranetMemberGroup> GetAll() => CurrentCache;

        public IntranetMemberGroup[] GetForMember(int id)
        {
            var allGroups = _memberGroupService.GetAll();
            var groupNamesAssignedToMember = _memberService.GetAllRoles(id);

            var memberGroups = allGroups
                .Join(groupNamesAssignedToMember, group => group.Name, identity, (group, _) => group)
                .Map<IntranetMemberGroup[]>();

            return memberGroups;
        }

        public int Create(string name)
        {
            _memberGroupService.Save(new MemberGroup() { Name = name });
            var group = _memberGroupService.GetByName(name);
            CurrentCache = CurrentCache.Append(group.Map<IntranetMemberGroup>());
            return group.Id;
        }

        public void Save(int id, string name)
        {
            var memberGroup = _memberGroupService.GetById(id);
            memberGroup.Name = name;
            _memberGroupService.Save(memberGroup);
            CurrentCache = CurrentCache.Where(i => i.Id != id).Append(memberGroup.Map<IntranetMemberGroup>());
        }

        public void Delete(int id)
        {
            var group = _memberGroupService.GetById(id);
            _memberGroupService.Delete(group);
            CurrentCache = CurrentCache.Where(i => i.Id != id);
        }
    }
}