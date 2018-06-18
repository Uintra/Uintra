using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Uintra.Core.Activity
{
    public interface IIntranetActivityRepository
    {
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