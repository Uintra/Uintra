using Umbraco.Core.Models;

namespace Uintra.Core.User
{
    public interface IIntranetUserContentProvider
    {
        IPublishedContent GetProfilePage();
        IPublishedContent GetEditPage();
    }
}
