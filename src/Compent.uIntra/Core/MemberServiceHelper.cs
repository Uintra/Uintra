using System.Collections.Generic;
using System.Linq;
using Uintra.Users;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using static LanguageExt.Prelude;


namespace Compent.Uintra.Core
{
    public class MemberServiceHelper : IMemberServiceHelper
    {
        private readonly IMemberService _memberService;
        private readonly IMemberTypeService _memberTypeService;

        public MemberServiceHelper(IMemberService memberService, IMemberTypeService memberTypeService)
        {
            _memberService = memberService;
            _memberTypeService = memberTypeService;
        }

        private const string RelatedUserPropertyName = "relatedUser";
        private const string FirstLoginPerformedPropertyName = "firstLoginPerformed";
        public Dictionary<IMember, int?> GetRelatedUserIdsForMembers(IEnumerable<IMember> users)
        {
            return users.ToDictionary(identity, u => u.GetValue<int?>(RelatedUserPropertyName));
        }

        public bool IsFirstLoginPerformed (IMember member) => member.GetValue<bool>(FirstLoginPerformedPropertyName);

        public void SetFirstLoginPerformed(IMember member)
        {
            member.SetValue(FirstLoginPerformedPropertyName, value: true);
            _memberService.Save(member, raiseEvents: false);
        }

        private const string MemberTypeAlias = "Member";
        private const string MemberProfileTabName = "Profile";
        public IEnumerable<PropertyType> GetAvailableProfileProperties()
        {
            var mts = _memberTypeService.Get(MemberTypeAlias);
            var profileTab = mts.PropertyGroups.FirstOrDefault(i => i.Name.Equals(MemberProfileTabName));
            var properties = profileTab.PropertyTypes
                .Where(i => !i.Alias.Equals(RelatedUserPropertyName));
            return properties;
        }
    }
}
