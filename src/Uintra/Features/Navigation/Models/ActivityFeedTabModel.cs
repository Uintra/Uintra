using System;
using Uintra.Core;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Navigation.Models
{
    public class ActivityFeedTabModel : TabModelBase
    {
        public Enum Type { get; set; }
        public bool HasSubscribersFilter { get; set; }
        public bool HasPinnedFilter { get; set; }        
        public IActivityCreateLinks Links { get; set; }
    }
}
