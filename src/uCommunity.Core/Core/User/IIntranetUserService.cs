using System;
using System.Collections.Generic;

namespace uCommunity.Core.User
{
    public interface IIntranetUserService
    {
        IIntranetUser Get(int umbracoId);
        IIntranetUser Get(Guid id);
        IEnumerable<IIntranetUser> GetAll();
        IEnumerable<IIntranetUser> GetMany(IEnumerable<Guid> ids);
        IEnumerable<IIntranetUser> GetMany(IEnumerable<int> ids);
        IIntranetUser GetCurrentUser();
        void FillCreator(IHaveCreator model);
    }
}