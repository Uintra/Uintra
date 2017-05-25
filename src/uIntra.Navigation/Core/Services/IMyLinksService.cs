using System;
using System.Collections.Generic;
using uIntra.Navigation.Core.Sql;

namespace uIntra.Navigation.Core.Services
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