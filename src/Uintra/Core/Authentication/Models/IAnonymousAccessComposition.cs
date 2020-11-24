using UBaseline.Shared.Property;

namespace Uintra.Core.Authentication.Models
{
    public interface IAnonymousAccessComposition
    {
        PropertyModel<bool> AllowAccess { get; set; }
    }
}