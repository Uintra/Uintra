using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEvents.Services.Contracts
{
    public interface IUmbracoContentPublishedEventService
    {
        void ProcessContentPublished(IContentService sender, PublishEventArgs<IContent> args);
    }
}