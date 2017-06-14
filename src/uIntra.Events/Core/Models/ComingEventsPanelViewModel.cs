using System.Collections.Generic;

namespace uIntra.Events.Core.Models
{
    public class ComingEventsPanelViewModel
    {
        public string Title { get; set; }
        public IEnumerable<ComingEventViewModel> Events { get; set; }
    }
}