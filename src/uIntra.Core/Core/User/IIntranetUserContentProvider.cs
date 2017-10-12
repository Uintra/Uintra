using Umbraco.Core.Models;

namespace uIntra.Core.User
{
    public interface IIntranetUserContentProvider
    {
        IPublishedContent GetProfilePage();
        IPublishedContent GetEditPage();
    }
}
