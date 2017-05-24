using System.Web;
using System.Web.Mvc;
using Compent.uCommunity.Core;
using uCommunity.Core.Activity;

namespace uCommunity.Core.Extentions
{
    public static class ViewDataExtensions
    {
        private const string DetailsPageUrl = "DetailsPageUrl";
        private const string OverviewPageUrl = "OverviewPageUrl";
        private const string GroupPageUrl = "GroupPageUrl";
        private const string CreatePageUrl = "CreatePageUrl";
        
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

        public static string GetActivityCreatePageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType)
        {
            return GetActivityPageUrl(dataView, activityType, OverviewPageUrl);
        }

        public static string GetActivityOverviewPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType)
        {
            return GetActivityPageUrl(dataView, activityType, OverviewPageUrl);
        }

        public static string GetActivityDetailsPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType)
        {
            return GetActivityPageUrl(dataView, activityType, DetailsPageUrl);
        }

        private static void SetActivityPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType, string pageName, string url)
        {
            dataView[$"{activityType}_{pageName}"] = url;
        }

        private static string GetActivityPageUrl(this ViewDataDictionary dataView, IntranetActivityTypeEnum activityType, string pageName)
        {
            var url = dataView[$"{activityType}_{pageName}"]?.ToString();

            return url;
        }
    }
}
