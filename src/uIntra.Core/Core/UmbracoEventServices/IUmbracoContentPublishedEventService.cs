using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;

namespace uIntra.Core.UmbracoEventServices
{
    public interface IUmbracoContentPublishedEventService
    {
        void ProcessContentPublished(IPublishingStrategy sender, PublishEventArgs<IContent> args);
    }
}