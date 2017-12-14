using System;

namespace uIntra.Tagging.UserTags.Models
{
    public class UserTagViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
    }
}
