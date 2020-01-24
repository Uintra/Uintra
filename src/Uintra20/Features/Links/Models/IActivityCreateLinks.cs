namespace Uintra20.Features.Links.Models
{
    public interface IActivityCreateLinks
    {
        UintraLinkModel Feed { get; }
        UintraLinkModel Overview { get; }
        UintraLinkModel Create { get; }
        UintraLinkModel Owner { get; }
        UintraLinkModel DetailsNoId { get; }
    }
}
