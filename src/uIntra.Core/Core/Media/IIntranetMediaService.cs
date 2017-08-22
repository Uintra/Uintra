using System;
using System.Collections.Generic;

namespace uIntra.Core.Media
{
    public interface IIntranetMediaService
    {
        void Create(Guid entityId, string mediaIds);
        IEnumerable<int> GetEntityMedia(Guid entityId);
        void Delete(Guid entityId);
        void Update(Guid entityId, string mediaIds);
        string GetEntityMediaString(Guid entityId);
    }
}
