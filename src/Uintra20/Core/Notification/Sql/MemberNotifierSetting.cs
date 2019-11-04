using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra20.Persistence;

namespace Uintra20.Core.Notification
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