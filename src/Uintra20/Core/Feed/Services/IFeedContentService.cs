using System;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra20.Core.Feed.Services
{
    public interface IFeedContentService
    {
        Enum GetCreateActivityType(IPublishedContent content);
    }
}