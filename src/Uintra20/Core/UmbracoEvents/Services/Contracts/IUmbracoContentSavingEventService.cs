using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra20.Core.UmbracoEvents.Services.Contracts
{
    public interface IUmbracoContentSavingEventService
    {
        void ProcessContentSaving(IContentService sender, SaveEventArgs<IContent> args);
    }
}
