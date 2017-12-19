using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace uIntra.Core.UmbracoEventServices
{
    public interface IUmbracoEventService <in TSender, in TArgs>
    {
        void Process(TSender sender, TArgs args);
    }

    public interface IUmbracoContentSavedEventService : IUmbracoEventService<IContentService, SaveEventArgs<IContent>>
    {}

    public interface IUmbracoContentPublishedEventService : IUmbracoEventService<IPublishingStrategy, PublishEventArgs<IContent>>
    {}

    public interface IUmbracoContentTrashedEventsService
    {
        void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> args);
    }
}
