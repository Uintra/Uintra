using System;
using System.Web.Mvc;

namespace uIntra.Core.Extensions
{
    public static class ViewDataExtensions
    {
        // activities page urls
        private const string DetailsPageUrl = "DetailsPageUrl";
        private const string OverviewPageUrl = "OverviewPageUrl";
        private const string CreatePageUrl = "CreatePageUrl";
        private const string EditPageUrl = "EditPageUrl";

        private const string ProfilePageUrl = "ProfilePageUrl";

        public static void SetActivityCreatePageUrl(this ViewDataDictionary dataView, int activityTypeId, string url)
        {
            SetActivityPageUrl(dataView, activityTypeId, CreatePageUrl, url);
        }

        public static void SetActivityOverviewPageUrl(this ViewDataDictionary dataView, int activityTypeId, string url)
        {
            SetActivityPageUrl(dataView, activityTypeId, OverviewPageUrl, url);
        }

        public static void SetActivityDetailsPageUrl(this ViewDataDictionary dataView, int activityTypeId, string url)
        {
            SetActivityPageUrl(dataView, activityTypeId, DetailsPageUrl, url);
        }

        public static void SetActivityEditPageUrl(this ViewDataDictionary dataView, int activityTypeId, string url)
        {
            SetActivityPageUrl(dataView, activityTypeId, EditPageUrl, url);
        }

        public static void SetProfilePageUrl(this ViewDataDictionary dataView, string url)
        {
            dataView[ProfilePageUrl] = url;
        }

        public static string GetActivityCreatePageUrl(this ViewDataDictionary dataView, int activityTypeId)
        {
            return GetActivityPageUrl(dataView, activityTypeId, CreatePageUrl);
        }

        public static string GetActivityOverviewPageUrl(this ViewDataDictionary dataView, int activityTypeId)
        {
            return GetActivityPageUrl(dataView, activityTypeId, OverviewPageUrl);
        }

        public static string GetActivityDetailsPageUrl(this ViewDataDictionary dataView, int activityTypeId)
        {
            return GetActivityPageUrl(dataView, activityTypeId, DetailsPageUrl);
        }

        public static string GetActivityDetailsPageUrl(this ViewDataDictionary dataView, int activityTypeId, Guid id)
        {
            return GetActivityPageUrl(dataView, activityTypeId, DetailsPageUrl).AddIdParameter(id);
        }

        public static string GetActivityEditPageUrl(this ViewDataDictionary dataView, int activityTypeId)
        {
            return GetActivityPageUrl(dataView, activityTypeId, EditPageUrl);
        }

        public static string GetActivityEditPageUrl(this ViewDataDictionary dataView, int activityTypeId, Guid id)
        {
            return GetActivityPageUrl(dataView, activityTypeId, EditPageUrl).AddIdParameter(id);
        }

        public static string GetProfilePageUrl(this ViewDataDictionary dataView, Guid id)
        {
           return dataView[ProfilePageUrl]?.ToString().AddIdParameter(id);
        }

        private static void SetActivityPageUrl(this ViewDataDictionary dataView, int activityTypeId, string pageName, string url)
        {
            dataView[GetKey(activityTypeId, pageName)] = url;
        }

        private static string GetActivityPageUrl(this ViewDataDictionary dataView, int activityTypeId, string pageName)
        {
            var url = dataView[GetKey(activityTypeId, pageName)]?.ToString();

            return url;
        }

        private static string GetKey(int activityTypeId, string pageName)
        {
            return $"{activityTypeId}_{pageName}";
        }
    }
}
