using System;
using Uintra.Core.Activity;
using Uintra.Core.Location;

namespace Uintra.News
{
    public class NewsViewModel : IntranetActivityViewModelBase
    {
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Media { get; set; }
    }
}