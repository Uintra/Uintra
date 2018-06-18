using System;

namespace Uintra.Tagging.UserTags.Models
{
    public class UserTag
    {
        public Guid Id { get; }
        public string Text { get; }

        public UserTag(Guid id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
