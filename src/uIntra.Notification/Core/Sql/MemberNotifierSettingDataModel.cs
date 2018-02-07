using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uIntra.Core.Persistence;

namespace uIntra.Notification
{
    [uIntraTable("MemberNotifiersSetting")]
    public class MemberNotifierSetting : SqlEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid MemberId { get; set; }
        public int NotifierType { get; set; }
        public bool IsEnabled { get; set; }
    }
}