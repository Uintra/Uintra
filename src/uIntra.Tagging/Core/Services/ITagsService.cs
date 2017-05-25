using System;
using System.Collections.Generic;

namespace uIntra.Tagging
{
    public interface ITagsService
    {
        IEnumerable<Tag> GetMany(Guid activityId);

        IEnumerable<Tag> FindAll(string query);

        IEnumerable<Tag> GetAll();

        void Save(Guid activityId, IEnumerable<TagDTO> tags);
    }
}
