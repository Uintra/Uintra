using System;
using System.Threading.Tasks;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Links
{
    public interface IActivityLinkService
    {
        IActivityLinks GetLinks(Guid activityId);

        Task<IActivityLinks> GetLinksAsync(Guid activityId);
    }
}
