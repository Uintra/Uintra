using System;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;

namespace uIntra.Bulletins
{
    public class BulletinPreviewViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public IIntranetUser Creator { get; set; }
        public IIntranetType ActivityType { get; set; }
    }
}