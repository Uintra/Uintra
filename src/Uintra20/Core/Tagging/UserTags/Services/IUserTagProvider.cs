using System;
using System.Collections.Generic;

namespace Uintra20.Core.Tagging.UserTags
{
    public interface IUserTagProvider
    {
        IEnumerable<UserTag> GetAll();
        UserTag Get(Guid tagId);
        IEnumerable<UserTag> Get(IEnumerable<Guid> tagIds);
    }
}
