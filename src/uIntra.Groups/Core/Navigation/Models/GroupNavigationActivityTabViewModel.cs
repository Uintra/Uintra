using System;
using Uintra.Core.Links;

namespace Uintra.Groups.Navigation.Models
{
    public class GroupNavigationActivityTabViewModel : GroupNavigationTabViewModelBase
    {
        public Enum Type { get; set; }        
        public IActivityCreateLinks Links { get; set; }
    }
}