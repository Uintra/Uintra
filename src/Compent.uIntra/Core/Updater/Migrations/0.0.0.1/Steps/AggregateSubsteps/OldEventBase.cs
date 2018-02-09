using System;

namespace Compent.Uintra.Installer.Migrations.OldSubscribeSettings
{
    public class OldEventBase
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PublishDate { get; set; }
        public bool CanSubscribe { get; set; }
        public Guid CreatorId { get; set; }
        public Guid OwnerId { get; set; }
        public int? UmbracoCreatorId { get; set; }
        public string LocationTitle { get; set; }
        public string LocationAddress { get; set; }
    }
}