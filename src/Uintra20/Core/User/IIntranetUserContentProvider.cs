using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Core.User
{
    public interface IIntranetUserContentProvider
    {
        IPublishedContent GetProfilePage();
        IPublishedContent GetEditPage();
    }
}
