using Uintra.Core.Activity.Models;
using Uintra.Core.Feed.Models;

namespace Uintra.Core.Feed.Services
{
    public interface IFeedPresentationService
    {
        IntranetActivityPreviewModelBase GetPreviewModel(IFeedItem feedItems, bool isGroupFeed);
    }
}