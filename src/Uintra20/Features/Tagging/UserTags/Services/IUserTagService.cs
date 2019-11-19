using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.Tagging.UserTags.Models;

namespace Uintra20.Features.Tagging.UserTags.Services
{
    public interface IUserTagService
    {
        IEnumerable<UserTag> Get(Guid entityId);
        void Replace(Guid entityId, IEnumerable<Guid> tagIds);
        void DeleteAllFor(Guid entityId);

        Task<IEnumerable<UserTag>> GetAsync(Guid entityId);
        Task ReplaceAsync(Guid entityId, IEnumerable<Guid> tagIds);
        Task DeleteAllForAsync(Guid entityId);
    }
}
