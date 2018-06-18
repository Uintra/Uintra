using uIntra.Core.Activity;
using uIntra.Core.Media;

namespace uIntra.News
{
    public interface INewsService<out TNews> : IIntranetActivityService<TNews> where TNews : NewsBase 
    {
        MediaSettings GetMediaSettings();
        bool IsExpired(INewsBase news);
    }
}