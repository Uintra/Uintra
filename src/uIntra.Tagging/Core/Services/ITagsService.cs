using System;
using System.Collections.Generic;

namespace uIntra.Tagging
{
    public interface ITagsService
    {
        IEnumerable<Tag> GetAll();

        IEnumerable<Tag> GetAllForActivity(Guid activityId);

        IEnumerable<Tag> FindAll(string query);

        void SaveRelations(Guid activityId, IEnumerable<TagDTO> tags);
    }
}
