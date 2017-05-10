using System;
using System.Collections.Generic;

namespace uCommunity.Tagging
{
    public interface ITagsService
    {
        IEnumerable<Tag> GetAll();

        void FillTags(IHaveTags activity);

        Tag Add(Guid activityId, string text, Guid? tagId = null);

        void Remove(Guid tagId, Guid activityId);
    }
}
