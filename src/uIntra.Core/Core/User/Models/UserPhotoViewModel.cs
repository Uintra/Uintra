using System;
using uIntra.Core.Links;

namespace uIntra.Core.User
{
    public class UserPhotoViewModel
    {
        public Guid Id { get; set; }
        public string Photo { get; set; }
        public ActivityLinks Links { get; set; }
    }
}
