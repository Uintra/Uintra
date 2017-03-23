using System;
using System.Collections.Generic;
using uCommunity.Core.Activity.Sql;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Core.Activity
{
    public class IntranetActivityService : IIntranetActivityService
    {
        private readonly ISqlRepository<IntranetActivityEntity> _sqlRepository;

        public IntranetActivityService(ISqlRepository<IntranetActivityEntity> sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }

        public IntranetActivityEntity Get(Guid id)
        {
            return _sqlRepository.Get(id);
        }

        public IEnumerable<IntranetActivityEntity> GetMany(IntranetActivityTypeEnum activityType)
        {
            return _sqlRepository.FindAll(a => a.Type == activityType);
        }

        public IntranetActivityTypeEnum GetType(Guid id)
        {
            var entity = _sqlRepository.Get(id);
            return entity.Type;
        }

        public void Create(IntranetActivityEntity entity)
        {
            _sqlRepository.Add(entity);
        }

        public void Update(IntranetActivityEntity entity)
        {
            _sqlRepository.Update(entity);
        }

        public void Delete(Guid id)
        {
            _sqlRepository.DeleteById(id);
        }
    }
}
