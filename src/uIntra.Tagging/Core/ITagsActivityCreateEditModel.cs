using System.Collections.Generic;

namespace uIntra.Tagging
{
    public interface ITagsActivityCreateEditModel
    {
        IList<TagEditModel> Tags { get; set; }
    }
}
