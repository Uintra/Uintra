using Uintra20.Core.Activity;
using Uintra20.Features.Media;
using Uintra20.Features.Media.Models;

namespace Uintra20.Features.News
{
    public interface INewsService<out TNews> : IIntranetActivityService<TNews> where TNews : NewsBase
    {
        MediaSettings GetMediaSettings();
        bool IsExpired(INewsBase news);
    }
}
