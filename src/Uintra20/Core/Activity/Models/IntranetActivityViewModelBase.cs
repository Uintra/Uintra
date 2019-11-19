using System;
using Uintra20.Core.Activity.Models.Headers;
using Uintra20.Features.Links.Models;
using Uintra20.Features.Location.Models;

namespace Uintra20.Core.Activity.Models
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