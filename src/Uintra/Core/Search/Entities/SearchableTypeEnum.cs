using Uintra.Core.Activity;

namespace Uintra.Core.Search.Entities
{
    public enum SearchableTypeEnum
    {
        News = IntranetActivityTypeEnum.News,
        Events = IntranetActivityTypeEnum.Events,
        Socials = IntranetActivityTypeEnum.Social,
        Content,
        Document
    }
}