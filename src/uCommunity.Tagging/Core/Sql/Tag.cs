using System;
using ServiceStack.DataAnnotations;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Tagging
{
    public class Tag : SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}
