using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEventServices
{
    public interface IUmbracoMediaSavingEventService
    {
        void ProcessMediaSaving(IMediaService sender, SaveEventArgs<IMedia> args);
    }
}
