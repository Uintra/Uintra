using System;

namespace uIntra.Groups
{
    public interface IGroupActivity 
    {
        Guid Id { get; set; }
        Guid? GroupId { get; set; }
        IGroupActivityHeader HeaderInfo { get; set; }
        bool IsReadonly { get; set; }
    }
}