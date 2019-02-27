using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.Core.UmbracoEventServices
{
    public interface IUmbracoContentSavingEventService
    {
        void ProcessContentSaving(IContentService sender, SaveEventArgs<IContent> args);
    }
}
