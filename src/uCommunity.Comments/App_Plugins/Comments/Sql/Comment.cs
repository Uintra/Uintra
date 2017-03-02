using System;
using ServiceStack.DataAnnotations;
using uCommunity.Core.App_Plugins.Core.Persistence.Sql;

namespace uCommunity.Comments.App_Plugins.Comments.Sql
{
    public class Comment : SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ActivityId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public string Text { get; set; }

        public Guid? ParentId { get; set; }
    }
}