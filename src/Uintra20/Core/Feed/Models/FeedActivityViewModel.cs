using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uintra20.Core.Member.Models;

namespace Uintra20.Core.Feed.Models
{
    public class FeedActivityViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public MemberViewModel Creator { get; set; }
        public string DateString { get; set; }
    }
}