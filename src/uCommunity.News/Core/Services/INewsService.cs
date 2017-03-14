using uCommunity.Core.Activity;
using uCommunity.Core.Media;

namespace uCommunity.News
{
    public interface INewsService : IIntranetActivityItemServiceBase<News>
    {
        MediaSettings GetMediaSettings();
    }
}