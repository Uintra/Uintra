using System;
using Uintra.Core.Links;
using Uintra.Core.User;

namespace Uintra.Events
{
    public class ComingEventViewModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IIntranetUser Owner { get; set; }
        public IActivityLinks Links { get; set; }
    }
}