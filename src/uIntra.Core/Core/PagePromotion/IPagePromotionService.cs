using uIntra.Core.Activity;

namespace uIntra.Core.PagePromotion
{
    public interface IPagePromotionService<out T> : IIntranetActivityService<T> where T : PagePromotionBase
    {
    }
}
