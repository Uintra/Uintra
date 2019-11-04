using System;
using Uintra20.Core.Links.Models;

namespace Uintra20.Core.Links
{
    public interface IActivityLinkService
    {
        IActivityLinks GetLinks(Guid activityId);
    }
}
