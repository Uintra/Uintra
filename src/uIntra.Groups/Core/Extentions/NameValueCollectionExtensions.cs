using System;
using System.Collections.Specialized;
using System.Linq;
using LanguageExt;
using Uintra.Groups.Constants;
using static LanguageExt.Prelude;

namespace Uintra.Groups.Extentions
{
    public static class NameValueCollectionExtensions
    {
        public static Option<Guid> GetGroupIdOrNone(this NameValueCollection query)
        {
            var id = query
                .Get(GroupConstants.GroupIdQueryParam)?
                .Split(',') // For case query String contains few groupIds
                .First();

            return parseGuid(id);
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