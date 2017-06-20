using System;
using System.Collections.Generic;

namespace uIntra.Core.User
{
    public interface IIntranetUserService<out T>
        where T : IIntranetUser
    {
        T Get(int umbracoId);
        T Get(Guid id);
        T Get(IHaveCreator model);
        IEnumerable<T> GetMany(IEnumerable<Guid> ids);
        IEnumerable<T> GetMany(IEnumerable<int> ids);
        IEnumerable<T> GetAll();
        T GetCurrentUser();
        IEnumerable<T> GetByRole(IntranetRolesEnum role);
    }
}