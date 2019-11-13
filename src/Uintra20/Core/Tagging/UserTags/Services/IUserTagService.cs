using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uintra20.Core.Tagging.UserTags
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
