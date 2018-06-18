using System.Collections.Generic;
using System.Linq;
using Uintra.Users;
using Umbraco.Core.Models;


namespace Compent.Uintra.Core
{
    public class MemberServiceHelper : IMemberServiceHelper
    {
        private const string RelatedUserPropertyName = "relatedUser";
        public Dictionary<IMember, int?> GetRelatedUserIdsForMembers(IEnumerable<IMember> users)
        {
            return users.ToDictionary(u => u, u => u.GetValue<int?>(RelatedUserPropertyName));
        }
    }
}
