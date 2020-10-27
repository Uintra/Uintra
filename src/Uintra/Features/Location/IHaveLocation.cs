using Uintra.Features.Location.Models;

namespace Uintra.Features.Location
{
    public interface IHaveLocation
    {
        ActivityLocation Location { get; set; }
    }
}
