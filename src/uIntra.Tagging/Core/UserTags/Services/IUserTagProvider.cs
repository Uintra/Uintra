using System;
using System.Collections.Generic;
using Uintra.Tagging.UserTags.Models;

namespace Uintra.Tagging.UserTags
{
    public interface IUserTagProvider
    {
        IEnumerable<UserTag> GetAll();
        UserTag Get(Guid tagId);
        IEnumerable<UserTag> Get(IEnumerable<Guid> tagIds);
    }
}