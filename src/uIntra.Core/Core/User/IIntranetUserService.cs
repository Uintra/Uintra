using System;
using System.Collections.Generic;

namespace uCommunity.Core.User
{
    public interface IIntranetUserService<out T>
        where T : IIntranetUser
    {
        T Get(int umbracoId);
        T Get(Guid id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(IEnumerable<Guid> ids);
        IEnumerable<T> GetMany(IEnumerable<int> ids);
        T GetCurrentUser();
        void FillCreator(IHaveCreator model);
        IEnumerable<T> GetByRole(IntranetRolesEnum role);
    }
}