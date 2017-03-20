using System;
using uCommunity.Core.Activity.Models;

namespace uCommunity.News
{
    public class NewsViewModelBase : IntranetActivityModelBase
    {
        public string Teaser { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }
    }
}