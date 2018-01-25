using System;
using uIntra.Core.Links;

namespace uIntra.Core.Activity
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
    }
}