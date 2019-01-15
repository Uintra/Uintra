using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Links;
using Uintra.Core.User;

namespace Uintra.Events
{
    public class EventPreviewViewModel
    {
        public string Title { get; set; }        
        public IEnumerable<string> Dates = Enumerable.Empty<string>();
        public UserViewModel Owner { get; set; }
        public Enum ActivityType { get; set; }
        public Guid Id { get; set; }
        public ActivityLinks Links { get; set; }
    }
}