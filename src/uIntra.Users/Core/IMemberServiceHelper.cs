using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Uintra.Users
{
    public  interface IMemberServiceHelper
    {
        Dictionary<IMember, int?> GetRelatedUserIdsForMembers(IEnumerable<IMember> users);
    }
}
