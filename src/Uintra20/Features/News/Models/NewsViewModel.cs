using System;
using Uintra20.Core.Activity.Models;

namespace Uintra20.Features.News.Models
{
    public class NewsViewModel : IntranetActivityViewModelBase
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }
    }
}