using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Core.Member.DTO;

namespace Uintra20.Core.Member
{
    public interface IIntranetMemberService<out T>
        where T : IIntranetMember
    {
        bool IsCurrentMemberSuperUser { get; }
        T GetByUserId(int umbracoId);
        T Get(int id);
        T Get(Guid id);
        T Get(IHaveOwner model);
        IEnumerable<T> GetMany(IEnumerable<Guid> ids);
        IEnumerable<T> GetMany(IEnumerable<int> ids);
        IEnumerable<T> GetAll();
        T GetCurrentMember();
        IEnumerable<T> GetByGroup(int groupId);
        T GetByName(string name);
        T GetByEmail(string email);
        bool Update(UpdateMemberDto dto);
        Guid Create(CreateMemberDto dto);
        ReadMemberDto Read(Guid id);
        bool Delete(Guid id);

        Task<bool> IsCurrentMemberSuperUserAsync();
        ITask<T> GetByUserIdAsync(int umbracoId);
        ITask<T> GetAsync(int id);
        ITask<T> GetAsync(Guid id);
        ITask<T> GetAsync(IHaveOwner model);
        ITask<IEnumerable<T>> GetManyAsync(IEnumerable<Guid> ids);
        ITask<IEnumerable<T>> GetManyAsync(IEnumerable<int> ids);
        ITask<IEnumerable<T>> GetAllAsync();
        ITask<T> GetCurrentMemberAsync();
        ITask<IEnumerable<T>> GetByGroupAsync(int groupId);
        ITask<T> GetByNameAsync(string name);
        ITask<T> GetByEmailAsync(string email);
        Task<bool> UpdateAsync(UpdateMemberDto dto);
        Task<Guid> CreateAsync(CreateMemberDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
