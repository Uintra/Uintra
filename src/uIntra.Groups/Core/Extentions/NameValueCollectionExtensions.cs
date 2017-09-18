using System;
using System.Collections.Specialized;
using uIntra.Groups.Constants;

namespace uIntra.Groups.Extentions
{
    public static class NameValueCollectionExtensions
    {
        public static Guid? GetGroupId(this NameValueCollection query)
        {
            var id = query.Get(GroupConstants.GroupIdQueryParam);
            Guid result;
            return Guid.TryParse(id, out result)
                ? (Guid?)result
                : null;
        }
    }
}