using System;
using System.Collections.Generic;

namespace uCommunity.Tagging
{
    public interface ITagsService
    {
        IEnumerable<Tag> GetAll();

        void FillTags(IHaveTags activity);

        void SaveTags(Guid activityId, IEnumerable<string> tags);
    }
}
