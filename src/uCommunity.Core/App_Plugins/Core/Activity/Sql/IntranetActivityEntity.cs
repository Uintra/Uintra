using System;
using ServiceStack.DataAnnotations;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Core.Activity.Sql
{
    [Alias("Activity")]
    public class IntranetActivityEntity : SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [StringLength(int.MaxValue)]
        public string JsonData { get; set; }

        public IntranetActivityTypeEnum Type { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}