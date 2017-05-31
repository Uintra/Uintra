using Umbraco.Core.Models;

namespace uIntra.Core.User
{
    public interface IIntranetUserContentHelper
    {
        IPublishedContent GetProfilePage();
        IPublishedContent GetEditPage();
    }
}
