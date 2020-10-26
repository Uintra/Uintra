using System;
using Umbraco.Core.Models.PublishedContent;

namespace Uintra.Core.Feed.Services
{
    public interface IFeedContentService
    {
        Enum GetCreateActivityType(IPublishedContent content);
    }
}