using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using uCommunity.Core.User;
using uCommunity.News.Base;

namespace uCommunity.News
{
    public class NewsCreateModel : NewsCreateEditModelBase
    {
        public IEnumerable<IntranetUserBase> Users { get; set; }

        public string AllowedMediaExtentions { get; set; }

        [Required]
        public Guid CreatorId { get; set; }
    }
}