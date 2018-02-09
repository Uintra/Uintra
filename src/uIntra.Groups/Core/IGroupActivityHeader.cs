using System;

namespace Uintra.Groups
{
    public interface IGroupActivityHeader
    {
        Guid? GroupId { get; set; }
        string GroupTitle { get; set; }
    }
}
