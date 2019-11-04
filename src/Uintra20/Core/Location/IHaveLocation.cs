using Uintra20.Core.Location.Models;

namespace Uintra20.Core.Location
{
    public interface IHaveLocation
    {
        ActivityLocation Location { get; set; }
    }
}
