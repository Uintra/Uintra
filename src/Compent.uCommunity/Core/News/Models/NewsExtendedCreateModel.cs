using System.Collections.Generic;
using uCommunity.News;
using uCommunity.Tagging;

namespace Compent.uCommunity.Core.News.Models
{
    public class NewsExtendedCreateModel : NewsCreateModel, ITagsCreateEditModel
    {
        public IEnumerable<string> Tags { get; set; }
    }
}