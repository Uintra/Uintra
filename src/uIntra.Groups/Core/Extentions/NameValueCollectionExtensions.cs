using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using uIntra.Groups.Constants;


namespace uIntra.Groups.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static Guid? GetGroupId(this NameValueCollection query)
        {
            var id = query
                .Get(GroupConstants.GroupIdQueryParam)?
                .Split(',') // For case query String contains few groupIds
                .First();

            return Guid.TryParse(id, out var result)
                ? new Guid?(result)
                : null;
        }
    }
}