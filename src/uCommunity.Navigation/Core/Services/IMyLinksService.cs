using System;
using System.Collections.Generic;

namespace uCommunity.Navigation.Core
{
    public interface IMyLinksService
    {
        MyLink Get(Guid id);

        IEnumerable<MyLink> GetMany(Guid userId);

        bool AddRemove(Guid userId, string name, string url);
    }
}