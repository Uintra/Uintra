using System.Collections.Generic;
using System.Linq;
using uIntra.Users;
using Umbraco.Core.Models;


namespace Compent.uIntra.Core
{
    public class MemberServiceHelper : IMemberServiceHelper
    {
        private const string RelatedUserPropertyName = "relatedUser";
        public Dictionary<IMember, int?> GetRelatedUserIdsForMembers(IEnumerable<IMember> users)
        {
            return users.ToDictionary(u => u, u => u.GetValue<int?>(nameof(RelatedUserPropertyName)));
        }
    }
}
