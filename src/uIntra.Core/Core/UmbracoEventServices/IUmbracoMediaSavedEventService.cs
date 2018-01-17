using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uIntra.Core.UmbracoEventServices
{
    public interface IUmbracoMediaSavedEventService
    {
        void ProcessMediaSaved(IMediaService sender, SaveEventArgs<IMedia> args);
    }
}