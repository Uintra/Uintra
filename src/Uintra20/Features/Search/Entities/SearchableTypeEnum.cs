using Uintra20.Core.Activity;

namespace Uintra20.Features.Search.Entities
{
    public enum SearchableTypeEnum
    {
        News = IntranetActivityTypeEnum.News,
        Events = IntranetActivityTypeEnum.Events,
        Bulletins = IntranetActivityTypeEnum.Social,
        Content,
        Document
    }
}