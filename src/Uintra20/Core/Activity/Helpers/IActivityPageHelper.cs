using System;

namespace Uintra20.Core.Activity.Helpers
{
    public interface IActivityPageHelper
    {
        Enum ActivityType { get; }
        string GetFeedUrl();
        //string GetOverviewPageUrl();//TODO: Research overview page
        string GetDetailsPageUrl(Guid? activityId = null);
        string GetCreatePageUrl();
        string GetEditPageUrl(Guid activityId);
    }
}