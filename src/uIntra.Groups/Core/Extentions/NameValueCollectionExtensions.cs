using System;
using System.Collections.Specialized;
using System.Linq;
using uIntra.Groups.Constants;

namespace uIntra.Groups.Extentions
{
    public static class NameValueCollectionExtensions
    {
        public static Guid? GetGroupId(this NameValueCollection query)
        {
            var id = query
                .Get(GroupConstants.GroupIdQueryParam)
                .Split(',') // For case query String contains few groupIds
                .First();

            Guid result;
            return Guid.TryParse(id, out result)
                ? (Guid?)result
                : null;
        }
    }
}