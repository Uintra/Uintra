using System.Collections.Generic;
using uCommunity.News;

namespace Compent.uCommunity.Core.News.Models
{
    public class NewsExtendedCreateModel : NewsCreateModel
    {
        public IEnumerable<string> Tags { get; set; }
    }
}