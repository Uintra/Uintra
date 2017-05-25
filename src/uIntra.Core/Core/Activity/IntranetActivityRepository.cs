using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Core.Activity
{
    public class IntranetActivityRepository : IIntranetActivityRepository
    {
        private readonly ISqlRepository<IntranetActivityEntity> _sqlRepository;

        public IntranetActivityRepository(ISqlRepository<IntranetActivityEntity> sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }

        public IntranetActivityEntity Get(Guid id)
        {
            return _sqlRepository.Get(id);
        }

        public IEnumerable<IntranetActivityEntity> GetAll()
        {
            return _sqlRepository.GetAll();
        }

        public IEnumerable<IntranetActivityEntity> GetMany(IntranetActivityTypeEnum activityType)
        {
            return _sqlRepository.FindAll(a => a.Type == activityType);
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
            _sqlRepository.DeleteById(id);
        }
    }
}
