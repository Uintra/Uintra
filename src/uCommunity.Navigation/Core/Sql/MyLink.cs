using System;
using ServiceStack.DataAnnotations;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Navigation.Core
{
    public class MyLink : SqlEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}