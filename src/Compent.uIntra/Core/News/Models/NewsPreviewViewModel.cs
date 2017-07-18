using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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