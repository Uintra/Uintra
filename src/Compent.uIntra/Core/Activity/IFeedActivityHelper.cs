using System;
using uIntra.Groups;

namespace Compent.uIntra.Core.Activity
{
    public interface IFeedActivityHelper
    {
        GroupInfo? GetGroupInfo(Guid activityId);
    }
}