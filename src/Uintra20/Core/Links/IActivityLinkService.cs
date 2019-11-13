using System;
using System.Threading.Tasks;

namespace Uintra20.Core.Links
{
    public interface IActivityLinkService
    {
        IActivityLinks GetLinks(Guid activityId);

        Task<IActivityLinks> GetLinksAsync(Guid activityId);
    }
}
