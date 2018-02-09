using System;
using Uintra.Groups;

namespace Compent.Uintra.Core.Activity
{
    public interface IFeedActivityHelper
    {
        GroupInfo? GetGroupInfo(Guid activityId);
    }
}