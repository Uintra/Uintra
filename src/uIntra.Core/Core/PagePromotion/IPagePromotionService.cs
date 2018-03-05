using Uintra.Core.Activity;

namespace Uintra.Core.PagePromotion
{
    public interface IPagePromotionService<out T> : IIntranetActivityService<T> where T : PagePromotionBase
    {
    }
}
