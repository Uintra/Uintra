using System;
using uIntra.Core.Extentions;
using Umbraco.Core.Models;

namespace uIntra.Groups.Extentions
{
    public static class PublishedContentExtentions
    {
        public const string GroupIdQueryParam = "groupId";

        public static string UrlWithGroupId(this IPublishedContent content, Guid? groupId)
        {
            return content.Url.UrlWithGroupId(groupId);
        }

        public static string UrlWithGroupId(this string url, Guid? groupId)
        {
            if (groupId == null)
            {
                return url;
            }

            return url.AddParameter(GroupIdQueryParam, groupId);
        }
    }
}
