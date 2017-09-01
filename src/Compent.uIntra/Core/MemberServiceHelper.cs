using System.Collections.Generic;
using System.Linq;
using uIntra.Users;
using Umbraco.Core.Models;
using Member = Umbraco.Web.PublishedContentModels.Member;


namespace Compent.uIntra.Core
{
    public class MemberServiceHelper : IMemberServiceHelper
    {
        public Dictionary<IMember, int?> GetRelatedUserIdsForMembers(IEnumerable<IMember> users)
        {
            return users.ToDictionary(u => u, u => u.GetValue<int?>(nameof(Member.RelatedUser)));
        }
    }
}
