using Uintra20.Core.Activity.Models;
using Uintra20.Features.LinkPreview.Models;

namespace Uintra20.Features.Social.Models
{
    public class SocialPreviewModel : IntranetActivityPreviewModelBase
    {
        public LinkPreviewViewModel LinkPreview { get; set; }
    }
}