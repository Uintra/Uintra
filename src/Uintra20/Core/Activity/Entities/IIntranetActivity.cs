using System;
using System.Collections.Generic;
using Uintra20.Features.Location;

namespace Uintra20.Core.Activity.Entities
{
    public interface IIntranetActivity : IHaveLocation
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
