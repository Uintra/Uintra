using System;
using System.Collections.Generic;

namespace uIntra.Tagging.UserTags
{
    public interface IUserTagService
    {
        IEnumerable<UserTag> GetRelatedTags(Guid entityId);
        void ReplaceRelations(Guid entityId, IEnumerable<Guid> tagIds);
    }
}
