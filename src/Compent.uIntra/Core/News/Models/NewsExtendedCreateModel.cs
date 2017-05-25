using System.Collections.Generic;
using uIntra.News;
using uIntra.Tagging;

namespace Compent.uIntra.Core.News.Models
{
    public class NewsExtendedCreateModel : NewsCreateModel, ITagsActivityCreateEditModel
    {
        public NewsExtendedCreateModel()
        {
            Tags = new List<TagEditModel>();
        }

        public IList<TagEditModel> Tags { get; set; }
    }
}