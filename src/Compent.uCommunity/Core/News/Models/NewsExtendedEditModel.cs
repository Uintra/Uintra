using System.Collections.Generic;
using uCommunity.News;

namespace Compent.uCommunity.Core.News.Models
{
    public class NewsExtendedEditModel : NewsEditModel
    {
        public IEnumerable<string> Tags { get; set; }
    }
}