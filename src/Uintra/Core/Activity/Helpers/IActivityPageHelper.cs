using System;
using Uintra.Features.Links.Models;

namespace Uintra.Core.Activity.Helpers
{
    public interface IActivityPageHelper
    {
        UintraLinkModel GetFeedUrl();
        UintraLinkModel GetDetailsPageUrl(Enum activityType, Guid? activityId = null);
        UintraLinkModel GetCreatePageUrl(Enum activityType);
        UintraLinkModel GetEditPageUrl(Enum activityType, Guid? activityId = null);
    }
}