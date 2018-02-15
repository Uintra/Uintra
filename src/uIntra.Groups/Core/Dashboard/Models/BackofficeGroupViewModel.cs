using System;

namespace Uintra.Groups.Dashboard
{
    public class BackofficeGroupViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreatorName { get; set; }
        public string Link { get; set; }
        public bool IsHidden { get; set; }
    }
}