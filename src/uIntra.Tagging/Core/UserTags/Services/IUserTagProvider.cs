using System;
using System.Collections.Generic;
using uIntra.Tagging.UserTags.Models;

namespace uIntra.Tagging.UserTags
{
    public interface IUserTagProvider
    {
        IEnumerable<UserTag> GetAll();
        UserTag Get(Guid tagId);
        IEnumerable<UserTag> Get(IEnumerable<Guid> tagIds);
    }
}