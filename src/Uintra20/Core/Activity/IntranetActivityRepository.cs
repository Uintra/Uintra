using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Uintra20.Core.Extensions;
using Uintra20.Persistence;

namespace Uintra20.Core.Activity
{
    public class IntranetActivityRepository : IIntranetActivityRepository
    {
        private readonly ISqlRepository<Guid, IntranetActivityEntity> _sqlRepository;

        public IntranetActivityRepository(ISqlRepository<Guid, IntranetActivityEntity> sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }

        #region async

        public async Task<IntranetActivityEntity> GetAsync(Guid id)
        {
            return await _sqlRepository.GetAsync(id);
        }

        public async Task<IEnumerable<IntranetActivityEntity>> GetAllAsync()
        {
            return await _sqlRepository.GetAllAsync();
        }

        public async Task<IEnumerable<IntranetActivityEntity>> GetManyAsync(Enum activityType)
        {
            var activityTypeId = activityType.ToInt();
            return await _sqlRepository.FindAllAsync(a => a.Type == activityTypeId);
        }

        public async Task<IEnumerable<IntranetActivityEntity>> FindAllAsync(Expression<Func<IntranetActivityEntity, bool>> expression)
        {
            return await _sqlRepository.FindAllAsync(expression);
        }

        public async Task<IntranetActivityEntity> FindAsync(Expression<Func<IntranetActivityEntity, bool>> expression)
        {
            return await _sqlRepository.FindAsync(expression);
        }

        public async Task<long> CountAsync(Expression<Func<IntranetActivityEntity, bool>> expression)
        {
            return await _sqlRepository.CountAsync(expression);
        }

        public async Task<bool> ExistAsync(Expression<Func<IntranetActivityEntity, bool>> expression)
        {
            return await _sqlRepository.ExistsAsync(expression);
        }

        public async Task CreateAsync(IntranetActivityEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.ModifyDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.UtcNow;
            await _sqlRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(IntranetActivityEntity entity)
        {
            entity.ModifyDate = DateTime.UtcNow;
            await _sqlRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _sqlRepository.DeleteAsync(id);
        }

        #endregion

        #region sync

        public IntranetActivityEntity Get(Guid id)
        {
            return _sqlRepository.Get(id);
        }

        public IEnumerable<IntranetActivityEntity> GetAll()
        {
            return _sqlRepository.GetAll();
        }

        public IEnumerable<IntranetActivityEntity> GetMany(Enum activityType)
        {
            var activityTypeId = activityType.ToInt();
            return _sqlRepository.FindAll(a => a.Type == activityTypeId);
        }

        public IEnumerable<IntranetActivityEntity> FindAll(Expression<Func<IntranetActivityEntity, bool>> expression)
        {
            return _sqlRepository.FindAll(expression);
        }

        public IntranetActivityEntity Find(Expression<Func<IntranetActivityEntity, bool>> expression)
        {
            return _sqlRepository.Find(expression);
        }

        public long Count(Expression<Func<IntranetActivityEntity, bool>> expression)
        {
            return _sqlRepository.Count(expression);
        }

        public bool Exist(Expression<Func<IntranetActivityEntity, bool>> expression)
        {
            return _sqlRepository.Exists(expression);
        }

        public void Create(IntranetActivityEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.ModifyDate = DateTime.UtcNow;
            entity.CreatedDate = DateTime.UtcNow;
            _sqlRepository.Add(entity);
        }

        public void Update(IntranetActivityEntity entity)
        {
            entity.ModifyDate = DateTime.UtcNow;
            _sqlRepository.Update(entity);
        }

        public void Delete(Guid id)
        {
            _sqlRepository.Delete(id);
        }

        #endregion
    }
}