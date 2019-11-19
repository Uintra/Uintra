using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Uintra20.Features.Activity.Sql;

namespace Uintra20.Features.Activity
{
    public interface IIntranetActivityRepository
    {
        Task<IntranetActivityEntity> GetAsync(Guid id);
        Task<IEnumerable<IntranetActivityEntity>> GetManyAsync(Enum activityType);
        Task<IEnumerable<IntranetActivityEntity>> FindAllAsync(Expression<Func<IntranetActivityEntity, bool>> expression);
        Task<IntranetActivityEntity> FindAsync(Expression<Func<IntranetActivityEntity, bool>> expression);
        Task CreateAsync(IntranetActivityEntity entity);
        Task UpdateAsync(IntranetActivityEntity entity);
        Task DeleteAsync(Guid id);
        Task<long> CountAsync(Expression<Func<IntranetActivityEntity, bool>> expression);
        Task<bool> ExistAsync(Expression<Func<IntranetActivityEntity, bool>> expression);
        Task<IEnumerable<IntranetActivityEntity>> GetAllAsync();

        IntranetActivityEntity Get(Guid id);
        IEnumerable<IntranetActivityEntity> GetMany(Enum activityType);
        IEnumerable<IntranetActivityEntity> FindAll(Expression<Func<IntranetActivityEntity, bool>> expression);
        IntranetActivityEntity Find(Expression<Func<IntranetActivityEntity, bool>> expression);
        void Create(IntranetActivityEntity entity);
        void Update(IntranetActivityEntity entity);
        void Delete(Guid id);
        long Count(Expression<Func<IntranetActivityEntity, bool>> expression);
        bool Exist(Expression<Func<IntranetActivityEntity, bool>> expression);
        IEnumerable<IntranetActivityEntity> GetAll();
    }
}
