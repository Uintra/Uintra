using System;
using ServiceStack.DataAnnotations;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Navigation.Core
{
    [CompositeIndex("UserId", "Url", Unique = true, Name = "UQ_MyLink_UserId_Url")]
    public class MyLink : SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int? SortOrder { get; set; }
    }
}