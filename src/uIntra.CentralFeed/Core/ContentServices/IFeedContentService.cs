using System;
using Umbraco.Core.Models;

namespace uIntra.CentralFeed
{
    public interface IFeedContentService
    {
        Enum GetCreateActivityType(IPublishedContent content);
        Enum GetFeedTabType(IPublishedContent content);
    }
}