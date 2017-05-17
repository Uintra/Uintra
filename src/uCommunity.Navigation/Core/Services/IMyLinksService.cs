using System;
using System.Collections.Generic;

namespace uCommunity.Navigation.Core
{
    public interface IMyLinksService
    {
        MyLink Get(Guid id);

        IEnumerable<MyLink> GetMany(IEnumerable<Guid> ids);

        IEnumerable<MyLink> GetUserLinks(Guid userId);

        bool Sort(Dictionary<Guid, int> sortOrders);

        bool AddRemove(Guid userId, string name, string url);
    }
}