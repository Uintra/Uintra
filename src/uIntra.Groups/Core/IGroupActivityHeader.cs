using System;

namespace uIntra.Groups
{
    public interface IGroupActivityHeader
    {
        Guid? GroupId { get; set; }
        string GroupTitle { get; set; }
    }
}
