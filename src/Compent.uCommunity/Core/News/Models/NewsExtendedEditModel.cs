using System.Collections.Generic;
using uCommunity.News;
using uCommunity.Tagging;

namespace Compent.uCommunity.Core.News.Models
{
    public class NewsExtendedEditModel : NewsEditModel, ITagsCreateEditModel
    {
        public IEnumerable<string> Tags { get; set; }
    }
}