using System;
using System.Collections.Generic;
using uIntra.Tagging.UserTags.Models;

namespace uIntra.Tagging.UserTags
{
    public interface IUserTagService
    {
        IEnumerable<UserTag> Get(Guid entityId);
        void Replace(Guid entityId, IEnumerable<Guid> tagIds);
        void DeleteAllFor(Guid entityId);
    }
}
