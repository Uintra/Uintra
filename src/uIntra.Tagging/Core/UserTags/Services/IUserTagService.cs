using System;
using System.Collections.Generic;
using Uintra.Tagging.UserTags.Models;

namespace Uintra.Tagging.UserTags
{
    public interface IUserTagService
    {
        IEnumerable<UserTag> Get(Guid entityId);
        void Replace(Guid entityId, IEnumerable<Guid> tagIds);
        void DeleteAllFor(Guid entityId);
    }
}
