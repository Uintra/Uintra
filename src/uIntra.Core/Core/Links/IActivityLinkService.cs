using System;

namespace Uintra.Core.Links
{
    public interface IActivityLinkService
    {
        IActivityLinks GetLinks(Guid activityId);
    }
}