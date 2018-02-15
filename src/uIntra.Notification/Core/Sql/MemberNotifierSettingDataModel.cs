using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Core.Persistence;

namespace Uintra.Notification
{
    [UintraTable("MemberNotifiersSetting")]
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