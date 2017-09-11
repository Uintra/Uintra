using System;
using uIntra.Core.Extentions;
using uIntra.Groups.Constants;
using Umbraco.Core.Models;

namespace uIntra.Groups.Extentions
{
    public static class GroupsPublishedContentExtensions
    {
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

            return url.AddParameter(GroupConstants.GroupIdQueryParam, groupId);
        }
    }
}
