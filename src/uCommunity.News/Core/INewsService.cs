using uCommunity.Core.Activity;
using uCommunity.Core.Media;

namespace uCommunity.News
{

    public interface INewsService<out TNews> : IIntranetActivityService<TNews> where TNews : NewsBase 
    {
        MediaSettings GetMediaSettings();
    }
}