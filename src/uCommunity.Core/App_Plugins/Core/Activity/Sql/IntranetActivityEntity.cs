using System;
using ServiceStack.DataAnnotations;
using uCommunity.Core.App_Plugins.Core.Persistence.Sql;

namespace uCommunity.Core.App_Plugins.Core.Activity.Entities
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

        public bool IsHidden { get; set; }
    }
}