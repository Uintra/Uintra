using System.Collections.Generic;

namespace uCommunity.Tagging
{
    public interface ITagsActivityCreateEditModel
    {
        IList<TagEditModel> Tags { get; set; }
    }
}
