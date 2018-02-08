using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uIntra.Core.UmbracoEventServices
{
    public interface IUmbracoMediaTrashedEventService
    {
        void ProcessMediaTrashed(IMediaService sender, MoveEventArgs<IMedia> args);
    }
}