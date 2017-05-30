using System;

namespace uIntra.Bulletins
{
    public class BulletinsBackofficeSaveModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public int UmbracoCreatorId { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsHidden { get; set; }
    }
}