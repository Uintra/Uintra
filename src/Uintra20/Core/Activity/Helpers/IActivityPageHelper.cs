using System;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.Activity.Helpers
{
    public interface IActivityPageHelper
    {
        UintraLinkModel GetFeedUrl();
        //string GetOverviewPageUrl();//TODO: Research overview page
        UintraLinkModel GetDetailsPageUrl(Enum activityType, Guid? activityId = null);
        UintraLinkModel GetCreatePageUrl(Enum activityType);
        UintraLinkModel GetEditPageUrl(Enum activityType, Guid activityId);
    }
}