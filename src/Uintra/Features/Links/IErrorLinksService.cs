using Uintra.Features.Links.Models;

namespace Uintra.Features.Links
{
    public interface IErrorLinksService
    {
        UintraLinkModel GetNotFoundPageLink();
        UintraLinkModel GetForbiddenPageLink();
    }
}
