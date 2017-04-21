using uCommunity.Core.Activity;
using uCommunity.Core.Media;

namespace uCommunity.News
{

    public interface INewsService : IIntranetActivityService<NewsBase>
    {
        MediaSettings GetMediaSettings();
    }
}