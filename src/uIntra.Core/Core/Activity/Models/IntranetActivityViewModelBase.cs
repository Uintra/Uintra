using System;
using uIntra.Core.TypeProviders;

namespace uIntra.Core.Activity
{
    public abstract class IntranetActivityViewModelBase
    {
        public Guid Id { get; set; }
        public bool CanEdit { get; set; }
        public bool IsPinned { get; set; }
        public IntranetActivityDetailsHeaderViewModel HeaderInfo { get; set; }
        public IIntranetType ActivityType { get; set; }
    }
}