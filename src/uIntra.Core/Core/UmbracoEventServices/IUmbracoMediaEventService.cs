using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uIntra.Core.UmbracoEventServices
{
    public interface IUmbracoMediaEventService
    {
        void ProcessMediaSaved(IMediaService sender, SaveEventArgs<IMedia> e);
    }
}