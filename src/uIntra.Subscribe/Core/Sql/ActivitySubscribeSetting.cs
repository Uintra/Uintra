using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Uintra.Core.Persistence;

namespace Uintra.Subscribe
{
    [UintraTable("ActivitySubscribeSetting")]
    public class ActivitySubscribeSetting : SqlEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        public Guid ActivityId { get; set; }

        public bool CanSubscribe { get; set; }

        public string SubscribeNotes { get; set; }
    }
}