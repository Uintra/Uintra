using System;

namespace Uintra20.Core.Tagging.UserTags
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