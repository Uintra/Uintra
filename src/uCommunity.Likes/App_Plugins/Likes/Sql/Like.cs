using System;
using ServiceStack.DataAnnotations;
using uCommunity.Core.App_Plugins.Core.Persistence.Sql;

namespace uCommunity.Likes.App_Plugins.Likes.Sql
{
    [CompositeIndex("UserId", "ActivityId", Unique = true, Name = "UQ_Like_UserId_ActivityId")]
    public class Like : SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ActivityId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}