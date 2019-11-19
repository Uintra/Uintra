namespace Uintra20.Features.Links.Models
{
    public interface IActivityLinks : IActivityCreateLinks
    {
        string Details { get; }
        string Edit { get; }
    }
}
