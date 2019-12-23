using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Uintra20.Core.Member.Helpers
{
    public  interface IMemberServiceHelper
    {
        Dictionary<IMember, int?> GetRelatedUserIdsForMembers(IEnumerable<IMember> users);
        bool IsFirstLoginPerformed(IMember member);
        void SetFirstLoginPerformed(IMember member);
        IEnumerable<PropertyType> GetAvailableProfileProperties();
    }
}
