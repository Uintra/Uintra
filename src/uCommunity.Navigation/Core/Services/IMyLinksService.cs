using System;
using System.Collections.Generic;

namespace uCommunity.Navigation.Core
{
    public interface IMyLinksService
    {
        MyLink Get(Guid id);

        IEnumerable<MyLink> GetMany(Guid userId);

        MyLink Create(Guid userId, string name, string url);

        void Delete(Guid id);
    }
}