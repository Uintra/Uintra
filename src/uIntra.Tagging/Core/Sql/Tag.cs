using System;
using ServiceStack.DataAnnotations;
using uIntra.Core.Persistence;

namespace uIntra.Tagging
{
    public class Tag : SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}
