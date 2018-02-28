using Uintra.Core.Activity;

namespace Uintra.CentralFeed
{
    public enum CentralFeedTypeEnum
    {
        All = 0,
        News = IntranetActivityTypeEnum.News,
        Events = IntranetActivityTypeEnum.Events,
        Bulletins = IntranetActivityTypeEnum.Bulletins,
        PagePromotion = IntranetActivityTypeEnum.PagePromotion
    }
}
