using System;
using Uintra.Features.CentralFeed.Models.GroupFeed;

namespace Uintra.Core.Activity
{
    public interface IFeedActivityHelper
    {
        GroupInfo? GetGroupInfo(Guid activityId);
    }
}
