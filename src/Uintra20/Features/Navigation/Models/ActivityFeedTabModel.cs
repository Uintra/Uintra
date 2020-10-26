using System;
using Uintra20.Core;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Navigation.Models
{
    public class ActivityFeedTabModel : TabModelBase
    {
        public Enum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }        
        public IActivityCreateLinks Links { get; set; }
    }
}
