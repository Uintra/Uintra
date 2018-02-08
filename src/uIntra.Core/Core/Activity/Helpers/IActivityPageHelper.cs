using System;

namespace uIntra.Core.Activity
{
    public interface IActivityPageHelper
    {
        Enum ActivityType { get; }
        string GetFeedUrl();
        string GetOverviewPageUrl();
        string GetDetailsPageUrl(Guid? activityId = null);
        string GetCreatePageUrl();
        string GetEditPageUrl(Guid activityId);
    }
}