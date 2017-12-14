using System;
using System.Collections.Generic;
using uIntra.Tagging.UserTags.Models;

namespace uIntra.Tagging.UserTags
{
    public interface IUserTagService
    {
        IEnumerable<UserTag> GetRelatedTags(Guid entityId);
        void ReplaceRelations(Guid entityId, IEnumerable<Guid> tagIds);
    }
}
