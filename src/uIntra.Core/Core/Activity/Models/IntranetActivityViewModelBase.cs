using System;
using Uintra.Core.Links;
using Uintra.Core.Location;

namespace Uintra.Core.Activity
{
    public abstract class IntranetActivityViewModelBase
    {
        public Guid Id { get; set; }
        public bool CanEdit { get; set; }
        public bool IsPinned { get; set; }
        public IntranetActivityDetailsHeaderViewModel HeaderInfo { get; set; }
        public Enum ActivityType { get; set; }
        public IActivityLinks Links { get; set; }
        public bool IsReadOnly { get; set; }
        public ActivityLocation Location { get; set; }
    }
}