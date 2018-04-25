using System.Collections.Generic;
using System.Linq;
using Uintra.Users;
using Umbraco.Core.Models;
using Umbraco.Core.Services;


namespace Compent.Uintra.Core
{
    public class MemberServiceHelper : IMemberServiceHelper
    {
        private readonly IMemberService _memberService;

        public MemberServiceHelper(IMemberService memberService)
        {
            _memberService = memberService;
        }

        private const string RelatedUserPropertyName = "relatedUser";
        private const string FirstLoginPerformedPropertyName = "firstLoginPerformed";
        public Dictionary<IMember, int?> GetRelatedUserIdsForMembers(IEnumerable<IMember> users)
        {
            return users.ToDictionary(u => u, u => u.GetValue<int?>(RelatedUserPropertyName));
        }

        public bool IsFirstLoginPerformed (IMember member) => member.GetValue<bool>(FirstLoginPerformedPropertyName);

        public void SetFirstLoginPerformed(IMember member)
        {
            member.SetValue(FirstLoginPerformedPropertyName, value: true);
            _memberService.Save(member, raiseEvents: false);
        }
    }
}
