using System;

namespace Compent.uIntra.Installer.Migrations
{
    internal class OldNotifierData
    {
        public Enum ActivityType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid NotifierId { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? CommentId { get; set; }
    }
}