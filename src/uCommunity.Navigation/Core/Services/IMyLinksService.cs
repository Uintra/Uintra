using System;
using System.Collections.Generic;
using uCommunity.Navigation.Core.Models;

namespace uCommunity.Navigation.Core
{
    public interface IMyLinksService
    {
        MyLink Get(Guid id);

        IEnumerable<MyLink> GetMany(IEnumerable<Guid> ids);

        IEnumerable<MyLink> GetMany(Guid userId);

        void Create(MyLinkDTO model);

        void Delete(MyLinkDTO model);

        bool Exists(MyLinkDTO model);
    }
}