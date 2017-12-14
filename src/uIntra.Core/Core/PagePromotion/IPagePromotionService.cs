using System;

namespace uIntra.Core.PagePromotion
{
    public interface IPagePromotionService<out T> where T : PagePromotionBase
    {
        T GetPagePromotion(Guid pageId);
    }
}
