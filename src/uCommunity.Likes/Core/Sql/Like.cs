using System;
using ServiceStack.DataAnnotations;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Likes
{
    [CompositeIndex("UserId", "EntityId", Unique = true, Name = "UQ_Like_UserId_EntityId")]
    public class Like : SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid EntityId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}