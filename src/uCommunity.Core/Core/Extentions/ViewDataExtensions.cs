using System.Web;
using System.Web.Mvc;
using Compent.uCommunity.Core;
using uCommunity.Core.Activity;

namespace uCommunity.Core.Extentions
{
    public static class ViewDataExtensions
    {
        private const string DefaultDateFormatAlias = "DefaultDateFormat";
        private const string DefaultTimeFormatAlias = "DefaultTimeFormat";
        private const string DefaultDateTimeFormatAlias = "DefaultDateTimeFormat";
        private const string DefaultDateTimeValuePickerFormatAlias = "DefaultDateTimeValuePickerFormat";
        private const string DefaultDatePickerFormatAlias = "DefaultDatePickerFormat";
        private const string DefaultDateTimePickerFormatAlias = "DefaultDateTimePickerFormat";

        private const string DetailsPageUrl = "DetailsPageUrl";
        private const string OverviewPageUrl = "OverviewPageUrl";
        private const string GroupPageUrl = "GroupPageUrl";
        private const string CreatePageUrl = "CreatePageUrl";

        public static void SetDateTimeFormats(this ViewDataDictionary viewData)
        {
            var dateTimeFormatProvider = HttpContext.Current.GetService<IDateTimeFormatProvider>();
            viewData[DefaultDateFormatAlias] = dateTimeFormatProvider.DefaultDateFormat;
            viewData[DefaultTimeFormatAlias] = dateTimeFormatProvider.DefaultTimeFormat;
            viewData[DefaultDateTimeFormatAlias] = dateTimeFormatProvider.DefaultDateTimeFormat;
            viewData[DefaultDateTimeValuePickerFormatAlias] = dateTimeFormatProvider.DefaultDateTimeValuePickerFormat;
            viewData[DefaultDatePickerFormatAlias] = dateTimeFormatProvider.DefaultDatePickerFormat;
            viewData[DefaultDateTimePickerFormatAlias] = dateTimeFormatProvider.DefaultDateTimePickerFormat;
        }

        public static string GetDefaultDateFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultDateFormatAlias]?.ToString();
        }

        public static string GetDefaultTimeFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultTimeFormatAlias]?.ToString();
        }

        public static string GetDefaultDateTimeFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultDateTimeFormatAlias]?.ToString();
        }

        public static string GetDefaultDatePickerFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultDatePickerFormatAlias]?.ToString();
        }

        public static string GetDefaultDateTimePickerFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultDateTimePickerFormatAlias]?.ToString();
        }

        public static string GetDefaultDateTimeValueFormat(this ViewDataDictionary viewData)
        {
            return viewData[DefaultDateTimeValuePickerFormatAlias]?.ToString();
        }

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
