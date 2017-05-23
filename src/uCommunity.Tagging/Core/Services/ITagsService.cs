using System;
using System.Collections.Generic;

namespace uCommunity.Tagging
{
    public interface ITagsService
    {
        IEnumerable<Tag> FindAll(string query);

        IEnumerable<Tag> GetAll();

        void Save(Guid activityId, IEnumerable<TagDTO> tags);

        void FillTags(IHaveTags activity);
    }
}
