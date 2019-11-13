using System;
using Uintra20.Core.Activity;
using Uintra20.Core.LinkPreview;

namespace Uintra20.Core.Bulletins
{
    public class BulletinViewModel : IntranetActivityViewModelBase
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }
        public LinkPreviewViewModel LinkPreview { get; set; }
    }
}