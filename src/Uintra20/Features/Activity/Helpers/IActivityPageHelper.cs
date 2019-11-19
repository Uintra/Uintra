using System;

namespace Uintra20.Features.Activity.Helpers
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