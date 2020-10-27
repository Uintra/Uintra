namespace Uintra.Features.Links.Models
{
    public interface IActivityLinks : IActivityCreateLinks
    {
        UintraLinkModel Details { get; }
        UintraLinkModel Edit { get; }
    }
}
