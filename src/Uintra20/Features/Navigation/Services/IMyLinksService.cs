using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra20.Features.Navigation.Models;
using Uintra20.Features.Navigation.Sql;

namespace Uintra20.Features.Navigation.Services
{
    public interface IMyLinksService
    {
        MyLink Get(Guid id);
        MyLink Get(MyLinkDTO model);
        IEnumerable<MyLink> GetMany(IEnumerable<Guid> ids);
        IEnumerable<MyLink> GetMany(Guid userId);
        Guid Create(MyLinkDTO model);
        void Delete(Guid id);
        void DeleteByActivityId(Guid activityId);

        Task<MyLink> GetAsync(Guid id);
        Task<MyLink> GetAsync(MyLinkDTO model);
        Task<IEnumerable<MyLink>> GetManyAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<MyLink>> GetManyAsync(Guid userId);
        Task<Guid> CreateAsync(MyLinkDTO model);
        Task DeleteAsync(Guid id);
        Task DeleteByActivityIdAsync(Guid activityId);
    }
}
