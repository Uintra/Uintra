using System;
using uIntra.Core.User;

namespace Compent.uIntra.Core.Bulletins
{
    public class BulletinPreviewViewModel
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public IIntranetUser Creator { get; set; }
    }
}