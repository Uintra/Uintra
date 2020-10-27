using Uintra.Core.Activity;
using Uintra.Features.Media;
using Uintra.Features.Media.Models;

namespace Uintra.Features.News
{
    public interface INewsService<out TNews> : IIntranetActivityService<TNews> where TNews : NewsBase
    {
        MediaSettings GetMediaSettings();
        bool IsExpired(INewsBase news);
    }
}
