using System;
using ServiceStack.DataAnnotations;
using uIntra.Core.Persistence.Sql;

namespace uCommunity.Subscribe
{
    [CompositeIndex("UserId", "ActivityId", Unique = true, Name = "UQ_Subscribe_UserId_ActivityId")]
    public class Subscribe : SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ActivityId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public bool IsNotificationDisabled { get; set; }
    }
}