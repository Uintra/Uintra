using System;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Features.CentralFeed
{
    public interface IFeedContentService
    {
        Enum GetCreateActivityType(IPublishedContent content);
    }
}