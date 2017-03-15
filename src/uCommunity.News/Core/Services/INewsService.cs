using uCommunity.Core.Activity;
using uCommunity.Core.Media;

namespace uCommunity.News
{
    public interface INewsService<in T, out TModel> : IIntranetActivityItemServiceBase<T, TModel>
           where T : NewsBase
           where TModel : NewsModelBase
    {
        MediaSettings GetMediaSettings();
    }
}