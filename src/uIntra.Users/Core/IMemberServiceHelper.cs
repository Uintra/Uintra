using System.Collections.Generic;
using Umbraco.Core.Models;

namespace uIntra.Users
{
    public  interface IMemberServiceHelper
    {
        Dictionary<IMember, int?> GetRelatedUserIdsForMembers(IEnumerable<IMember> users);
    }
}
