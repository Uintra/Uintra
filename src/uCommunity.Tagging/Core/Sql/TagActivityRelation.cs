using System;
using ServiceStack.DataAnnotations;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Tagging
{
    [CompositeIndex("TagId", "ActivityId", Unique = true, Name = "UQ_TagActivityRelation_TagId_ActivityId")]
    public class TagActivityRelation : SqlEntity
    {
        [Required]
        public Guid TagId { get; set; }

        [Required]
        public Guid ActivityId { get; set; }
    }
}
