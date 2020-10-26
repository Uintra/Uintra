using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Uintra20.Features.Media.Intranet.Services.Contracts
{
    public interface IIntranetMediaService
    {
        void Create(Guid entityId, string mediaIds);
        IEnumerable<int> GetEntityMedia(Guid entityId);
        void Delete(Guid entityId);
        void Update(Guid entityId, string mediaIds);
        Guid GetEntityIdByMediaId(int mediaId);
        string GetEntityMediaString(Guid entityId);

        Task CreateAsync(Guid entityId, string mediaIds);
        Task<IEnumerable<int>> GetEntityMediaAsync(Guid entityId);
        Task DeleteAsync(Guid entityId);
        Task UpdateAsync(Guid entityId, string mediaIds);
        Task<Guid> GetEntityIdByMediaIdAsync(int mediaId);
        Task<string> GetEntityMediaStringAsync(Guid entityId);
    }
}
