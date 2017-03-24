using System;
using System.Collections.Generic;

namespace uCommunity.Core.User
{
    public interface IIntranetUserService
    {
        IIntranetUser Get(int umbracoId);
        IIntranetUser GetCurrentUser();
        IEnumerable<IIntranetUser> GetMany(IEnumerable<Guid> ids);
        IEnumerable<IIntranetUser> GetAll();
        void FillCreator(IHaveCreator model);
    }
}