using System.Collections.Generic;
using uCommunity.News;
using uCommunity.Tagging;

namespace Compent.uIntra.Core.News.Models
{
    public class NewsExtendedEditModel : NewsEditModel, ITagsActivityCreateEditModel
    {
        public NewsExtendedEditModel()
        {
            Tags = new List<TagEditModel>();
        }

        public IList<TagEditModel> Tags { get; set; }
    }
}