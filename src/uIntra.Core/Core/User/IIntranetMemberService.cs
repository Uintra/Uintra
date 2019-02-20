using System;
using System.Collections.Generic;
using LanguageExt;
using Uintra.Core.User.DTO;

namespace Uintra.Core.User
{
    public interface IIntranetMemberService<out T>
        where T : IIntranetMember
    {
        bool IsCurrentMemberSuperUser { get; }
        T Get(int umbracoId);
        T Get(Guid id);
        T Get(IHaveOwner model);
        IEnumerable<T> GetMany(IEnumerable<Guid> ids);
        IEnumerable<T> GetMany(IEnumerable<int> ids);
        IEnumerable<T> GetAll();
        T GetCurrentMember();
        IEnumerable<T> GetByGroup(int role);
        T GetByName(string name);
        T GetByEmail(string email);
        bool Update(UpdateMemberDto dto);
        Guid Create(CreateMemberDto dto);
        Option<ReadMemberDto> Read(Guid id);
        bool Delete(Guid id);
    }
}