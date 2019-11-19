using System;
using System.Collections.Generic;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.Tagging.UserTags.Services
{
    public interface IUserTagProvider
    {
        IEnumerable<UserTag> GetAll();
        UserTag Get(Guid tagId);
        IEnumerable<UserTag> Get(IEnumerable<Guid> tagIds);
    }
}
