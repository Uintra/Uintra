using System.Collections.Generic;

namespace uCommunity.Tagging
{
    public interface ITagsCreateEditModel
    {
        IEnumerable<string> Tags { get; set; }
    }
}
