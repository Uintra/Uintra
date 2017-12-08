using System;
using uIntra.Core.TypeProviders;

namespace Compent.uIntra.Core.Migration
{
    internal class OldNotifierData
    {
        public IntranetType ActivityType { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public Guid NotifierId { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? CommentId { get; set; }
    }
}