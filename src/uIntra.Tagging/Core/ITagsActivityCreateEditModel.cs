using System.Collections.Generic;
using uIntra.Tagging.Core.Models;

namespace uIntra.Tagging
{
    public interface ITagsActivityCreateEditModel
    {
        IList<TagEditModel> Tags { get; set; }
    }
}
