using System;
using Uintra.Core.Extensions;
using Uintra.Groups.Constants;
using Umbraco.Core.Models;

namespace Uintra.Groups.Extentions
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
