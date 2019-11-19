using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Core.Member
{
    public interface IIntranetUserContentProvider
    {
        IPublishedContent GetProfilePage();
        IPublishedContent GetEditPage();
    }
}
