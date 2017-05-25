using System;
using System.Collections.Generic;

namespace uIntra.Navigation
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