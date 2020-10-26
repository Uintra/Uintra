namespace Uintra20.Features.Links.Models
{
    public class ActivityCreateLinks : IActivityCreateLinks
    {
        public UintraLinkModel Feed { get; set; }
        public UintraLinkModel Create { get; set; }
        public UintraLinkModel Owner { get; set; }
    }
}