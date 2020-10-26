using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Core.Member.Models.Dto;

namespace Uintra20.Core.Member.Services
{
    public interface IIntranetMemberService<T>
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
        Task<T> GetByUserIdAsync(int umbracoId);
        Task<T> GetAsync(int id);
        Task<T> GetAsync(Guid id);
        Task<T> GetAsync(IHaveOwner model);
        Task<IEnumerable<T>> GetManyAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<T>> GetManyAsync(IEnumerable<int> ids);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetCurrentMemberAsync();
        Task<IEnumerable<T>> GetByGroupAsync(int groupId);
        Task<T> GetByNameAsync(string name);
        Task<T> GetByEmailAsync(string email);
        Task<bool> UpdateAsync(UpdateMemberDto dto);
		Task<Guid> CreateAsync(CreateMemberDto dto);
		Task<bool> DeleteAsync(Guid id);
	}
}
