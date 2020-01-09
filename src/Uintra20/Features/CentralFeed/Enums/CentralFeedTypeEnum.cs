using Uintra20.Core.Activity;

namespace Uintra20.Features.CentralFeed.Enums
{
    public enum CentralFeedTypeEnum
    {
        All = 0,
        News = IntranetActivityTypeEnum.News,
        Events = IntranetActivityTypeEnum.Events,
        Social = IntranetActivityTypeEnum.Social,
        PagePromotion = IntranetActivityTypeEnum.PagePromotion
    }
}