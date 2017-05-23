using System;
using System.Collections.Generic;

namespace uCommunity.Tagging
{
    public interface ITagsService
    {
        IEnumerable<Tag> GetAll();

        void FillTags(IHaveTags activity);

        void Save(Guid activityId, IEnumerable<TagDTO> tags);

        IEnumerable<Tag> FindAll(string query);
    }
}
