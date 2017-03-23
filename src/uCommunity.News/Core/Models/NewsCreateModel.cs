using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using uCommunity.Core.User;

namespace uCommunity.News
{
    public class NewsCreateModel : NewsCreateEditModelBase
    {
        public IEnumerable<IIntranetUser> Users { get; set; }

        public string AllowedMediaExtentions { get; set; }

        [Required]
        public Guid CreatorId { get; set; }
    }
}