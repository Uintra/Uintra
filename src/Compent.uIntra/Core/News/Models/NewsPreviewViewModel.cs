using System;
using uIntra.Core.User;

namespace Compent.uIntra.Core.News.Models
{
    public class NewsPreviewViewModel
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public IIntranetUser Creator { get; set; }
    }
}