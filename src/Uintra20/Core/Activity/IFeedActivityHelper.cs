using System;
using Uintra20.Features.CentralFeed.Models.GroupFeed;

namespace Uintra20.Core.Activity
{
    public interface IFeedActivityHelper
    {
        GroupInfo? GetGroupInfo(Guid activityId);
    }
}
