namespace Uintra20.Core.Links.Models
{
    public interface IActivityLinks : IActivityCreateLinks
    {
        string Details { get; }
        string Edit { get; }
    }
}
