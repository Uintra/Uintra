using System;

namespace Uintra20.Core.Groups
{
    public interface IGroupActivity
    {
        Guid Id { get; set; }
        Guid? GroupId { get; set; }
        Guid CreatorId { get; set; }
    }
}
