using System;
using ServiceStack.DataAnnotations;
using uIntra.Core.Persistence.Sql;

namespace uIntra.Core.Activity.Sql
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