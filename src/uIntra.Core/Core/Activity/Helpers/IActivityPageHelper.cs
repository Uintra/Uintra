using System;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
{
    public interface IActivityPageHelper
    {
        IIntranetType ActivityType { get; }
        string GetFeedUrl();
        string GetOverviewPageUrl();
        string GetDetailsPageUrl(Guid? activityId = null);
        string GetCreatePageUrl();
        string GetEditPageUrl(Guid activityId);
    }
}