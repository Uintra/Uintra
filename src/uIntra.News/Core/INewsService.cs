using Uintra.Core.Activity;
using Uintra.Core.Media;

namespace Uintra.News
{
    public interface INewsService<out TNews> : IIntranetActivityService<TNews> where TNews : NewsBase 
    {
        MediaSettings GetMediaSettings();
        bool IsExpired(INewsBase news);
    }
}