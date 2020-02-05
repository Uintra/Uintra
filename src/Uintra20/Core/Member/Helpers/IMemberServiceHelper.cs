using System.Collections.Generic;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Models;
using Umbraco.Core.Models;

namespace Uintra20.Core.Member.Helpers
{
    public  interface IMemberServiceHelper
    {
        Dictionary<IMember, int?> GetRelatedUserIdsForMembers(IEnumerable<IMember> users);
        bool IsFirstLoginPerformed(IMember member);
        void SetFirstLoginPerformed(IMember member);
        IEnumerable<PropertyType> GetAvailableProfileProperties();
        MemberViewModel ToViewModel(IIntranetMember member);
    }
}
