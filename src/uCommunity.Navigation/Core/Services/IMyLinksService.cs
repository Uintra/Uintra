using System;
using System.Collections.Generic;

namespace uCommunity.Navigation.Core
{
    public interface IMyLinksService
    {
        MyLink Get(Guid id);

        IEnumerable<MyLink> GetMany(IEnumerable<Guid> ids);

        IEnumerable<MyLink> GetMany(Guid userId);

        void Create(Guid userId, int contentId);

        void Delete(Guid id);

        bool Exists(Guid userId, int contentId);
    }
}