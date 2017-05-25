using System.Collections.Generic;
using uIntra.News;
using uIntra.Tagging;

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