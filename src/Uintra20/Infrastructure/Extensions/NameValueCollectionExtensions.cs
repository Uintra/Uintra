using System;
using System.Collections.Specialized;
using System.Linq;
using Uintra20.Features.Groups.Constants;

namespace Uintra20.Infrastructure.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static Guid? GetGroupIdOrNone(this NameValueCollection query)
        {
            var id = query
                .Get(GroupConstants.GroupIdQueryParam)?
                .Split(',') // For case query String contains few groupIds
                .First();

            if (Guid.TryParse(id, out Guid result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public static Guid GetGroupId(this NameValueCollection query)
        {
            var id = query
                .Get(GroupConstants.GroupIdQueryParam)?
                .Split(',') // For case query String contains few groupIds
                .First();

            return Guid.Parse(id);
        }
    }
}