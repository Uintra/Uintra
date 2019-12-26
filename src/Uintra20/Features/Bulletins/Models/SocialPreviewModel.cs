using System.Collections.Generic;
using Uintra20.Core.Activity.Models;
using Uintra20.Features.LinkPreview.Models;

namespace Uintra20.Features.Bulletins.Models
{
    public class SocialPreviewModel : IntranetActivityPreviewModelBase
    {
        public LinkPreviewViewModel LinkPreview { get; set; }
        
    }
}