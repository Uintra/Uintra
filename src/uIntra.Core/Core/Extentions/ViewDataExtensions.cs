using System;
using System.Web.Mvc;
using uIntra.Core.Activity;

namespace uIntra.Core.Extentions
{
    public static class ViewDataExtensions
    {
        private const string DetailsPageUrl = "DetailsPageUrl";
        private const string OverviewPageUrl = "OverviewPageUrl";
        private const string CreatePageUrl = "CreatePageUrl";
        private const string EditPageUrl = "EditPageUrl";

        public static void SetActivityCreatePageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType, string url)
        {
            SetActivityPageUrl(dataView, activityType, CreatePageUrl, url);
        }

        public static void SetActivityOverviewPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType, string url)
        {
            SetActivityPageUrl(dataView, activityType, OverviewPageUrl, url);
        }

        public static void SetActivityDetailsPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType, string url)
        {
            SetActivityPageUrl(dataView, activityType, DetailsPageUrl, url);
        }

        public static void SetActivityEditPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType, string url)
        {
            SetActivityPageUrl(dataView, activityType, EditPageUrl, url);
        }

        public static string GetActivityCreatePageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType)
        {
            return GetActivityPageUrl(dataView, activityType, CreatePageUrl);
        }

        public static string GetActivityOverviewPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType)
        {
            return GetActivityPageUrl(dataView, activityType, OverviewPageUrl);
        }

        public static string GetActivityDetailsPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType)
        {
            return GetActivityPageUrl(dataView, activityType, DetailsPageUrl);
        }

        public static string GetActivityDetailsPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType, Guid id)
        {
            return GetActivityPageUrl(dataView, activityType, DetailsPageUrl).AddIdParameter(id);
        }

        public static string GetActivityEditPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType)
        {
            return GetActivityPageUrl(dataView, activityType, EditPageUrl);
        }

        public static string GetActivityEditPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType, Guid id)
        {
            return GetActivityPageUrl(dataView, activityType, EditPageUrl).AddIdParameter(id);
        }

        private static void SetActivityPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType, string pageName, string url)
        {
            dataView[GetKey(activityType, pageName)] = url;
        }

        private static string GetActivityPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType, string pageName)
        {
            var url = dataView[GetKey(activityType, pageName)]?.ToString();

            return url;
        }

        private static string GetKey(IntranetActivityTypeEnum activityType, string pageName)
        {
            return $"{activityType}_{pageName}";
        }
    }
}
