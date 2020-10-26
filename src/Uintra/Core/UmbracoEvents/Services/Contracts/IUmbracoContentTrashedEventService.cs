using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEvents.Services.Contracts
{
    public interface IUmbracoContentTrashedEventService
    {
        void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> args);
    }
}