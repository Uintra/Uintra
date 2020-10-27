using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEvents.Services.Contracts
{
    public interface IUmbracoMediaTrashedEventService
    {
        void ProcessMediaTrashed(IMediaService sender, MoveEventArgs<IMedia> args);
    }
}