using System;
using System.Collections.Generic;
using Uintra.Core.User.DTO;

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
        void Update(UpdateUserDto dto);
        Guid Create(CreateUserDto dto);
        ReadUserDto Read(Guid id);
        void Delete(Guid id);
    }
}