using Uintra20.Features.Location.Models;

namespace Uintra20.Features.Location
{
    public interface IHaveLocation
    {
        ActivityLocation Location { get; set; }
    }
}
