using Uintra20.Core.Activity.Models;
using Uintra20.Core.Feed.Models;

namespace Uintra20.Core.Feed.Services
{
    public interface IFeedPresentationService
    {
        IntranetActivityPreviewModelBase GetPreviewModel(IFeedItem feedItems, bool isGroupFeed);
    }
}