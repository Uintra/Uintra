using System;

namespace uCommunity.Core.Activity.Models
{
    public abstract class IntranetActivityViewModelBase
    {
        public Guid Id { get; set; }
        public bool CanEdit { get; set; }

        public IntranetActivityDetailsHeaderViewModel HeaderInfo { get; set; }
    }
}