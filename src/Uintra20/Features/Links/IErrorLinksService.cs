using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Links
{
    public interface IErrorLinksService
    {
        UintraLinkModel GetNotFoundPageLink();
        UintraLinkModel GetForbiddenPageLink();
    }
}
