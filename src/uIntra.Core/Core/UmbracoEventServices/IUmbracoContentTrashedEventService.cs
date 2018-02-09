using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Uintra.Core.UmbracoEventServices
{
    public interface IUmbracoContentTrashedEventService
    {
        void ProcessContentTrashed(IContentService sender, MoveEventArgs<IContent> args);
    }
}