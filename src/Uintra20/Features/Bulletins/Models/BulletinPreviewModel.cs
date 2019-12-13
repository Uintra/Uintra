using Uintra20.Core.Activity.Models;
using Uintra20.Features.LinkPreview.Models;

namespace Uintra20.Features.Bulletins.Models
{
    public class BulletinPreviewModel : IntranetActivityPreviewModelBase
    {
        public string Description { get; set; }
        public string Media { get; set; }
        public LinkPreviewViewModel LinkPreview { get; set; }
        
    }
}