using System;
using uIntra.Core.Extensions;
using uIntra.Groups.Constants;
using Umbraco.Core.Models;

namespace uIntra.Groups.Extensions
{
    public static class GroupsPublishedContentExtensions
    {
        public static string UrlWithGroupId(this IPublishedContent content, Guid? groupId)
        {
            return content.Url.UrlWithGroupId(groupId);
        }

        public static string UrlWithGroupId(this string url, Guid? groupId)
        {
            return groupId == null ? url : url.AddParameter(GroupConstants.GroupIdQueryParam, groupId);
        }
    }
}
