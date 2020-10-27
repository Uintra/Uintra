using System;
using System.Threading.Tasks;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Links
{
    public interface IActivityLinkService
    {
        IActivityLinks GetLinks(Guid activityId);

        Task<IActivityLinks> GetLinksAsync(Guid activityId);
    }
}
