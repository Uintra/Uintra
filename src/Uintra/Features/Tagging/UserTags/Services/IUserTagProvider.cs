using System;
using System.Collections.Generic;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Features.Tagging.UserTags.Services
{
    public interface IUserTagProvider
    {
        IEnumerable<UserTag> GetAll();
        UserTag Get(Guid tagId);
        IEnumerable<UserTag> Get(IEnumerable<Guid> tagIds);
    }
}
