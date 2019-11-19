using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.User
{
    public interface IIntranetUserContentProvider
    {
        IPublishedContent GetProfilePage();
        IPublishedContent GetEditPage();
    }
}
