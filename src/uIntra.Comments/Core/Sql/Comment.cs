using System;
using ServiceStack.DataAnnotations;
using uIntra.Core.Persistence;

namespace uIntra.Comments
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