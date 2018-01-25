using System;
using System.Collections.Generic;

namespace uIntra.Core.Activity
{
    public interface IIntranetActivity
    {
        Guid Id { get; set; }
        Enum Type { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime ModifyDate { get; set; }
        bool IsPinActual { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        bool IsHidden { get; set; }
        bool IsPinned { get; set; }
        DateTime? EndPinDate { get; set; }
        IEnumerable<int> MediaIds { get; set; }
    }
}