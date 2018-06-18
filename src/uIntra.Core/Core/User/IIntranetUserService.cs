using System;
using System.Collections.Generic;

namespace Uintra.Core.User
{
    public interface IIntranetUserService<out T>
        where T : IIntranetUser
    {
        T Get(int umbracoId);
        T Get(Guid id);
        T Get(IHaveOwner model);
        IEnumerable<T> GetMany(IEnumerable<Guid> ids);
        IEnumerable<T> GetMany(IEnumerable<int> ids);
        IEnumerable<T> GetAll();
        T GetCurrentUser();
        IEnumerable<T> GetByRole(int role);
        T GetByName(string name);
        T GetByEmail(string email);
        void Save(IntranetUserDTO user);
    }
}