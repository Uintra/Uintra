using System;

namespace uIntra.Core.User
{
    public interface IIntranetUser
    {
        Guid Id { get; set; }
        int? UmbracoId { get; set; }
        string DisplayedName { get; set; }
        string Photo { get; set; }
        IRole Role { get; set; }
    }
}
