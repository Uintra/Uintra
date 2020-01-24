using System;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.Activity.Helpers
{
    public interface IActivityPageHelper
    {
        Enum ActivityType { get; }
        UintraLinkModel GetFeedUrl();
        //string GetOverviewPageUrl();//TODO: Research overview page
        UintraLinkModel GetDetailsPageUrl(Guid? activityId = null);
        UintraLinkModel GetCreatePageUrl();
        UintraLinkModel GetEditPageUrl(Guid activityId);
    }
}