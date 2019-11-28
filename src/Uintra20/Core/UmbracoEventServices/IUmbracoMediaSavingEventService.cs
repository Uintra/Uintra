using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEventServices
{
    public interface IUmbracoMediaSavingEventService
    {
        void ProcessMediaSaving(IMediaService sender, SaveEventArgs<IMedia> args);
    }
}
