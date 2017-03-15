using System;
using System.Collections.Generic;
using uCommunity.Core.Activity.Sql;

namespace uCommunity.Core.Activity
{
    public interface IIntranetActivityService
    {
        IntranetActivityEntity Get(Guid id);
        IEnumerable<IntranetActivityEntity> GetMany(IntranetActivityTypeEnum activityType);
        IntranetActivityTypeEnum GetType(Guid id);
        void Create(IntranetActivityEntity entity);
        void Update(IntranetActivityEntity entity);
        void Delete(Guid id);
    }
}