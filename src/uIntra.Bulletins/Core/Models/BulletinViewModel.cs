using System;
using uIntra.Core.Activity;

namespace uIntra.Bulletins
{
    public class BulletinViewModel : IntranetActivityViewModelBase
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }
    }
}