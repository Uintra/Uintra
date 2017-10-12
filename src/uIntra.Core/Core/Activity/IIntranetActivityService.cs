using System;
using System.Collections.Generic;

namespace uIntra.Core.Activity
{
    public interface IIntranetActivityService<out TActivity> : IIntranetActivityService where TActivity : IIntranetActivity
    {
        TActivity Get(Guid id);
        IEnumerable<TActivity> GetManyActual();
        IEnumerable<TActivity> GetAll(bool includeHidden = false);
        bool IsActual(IIntranetActivity cachedActivity);        
        Guid Create(IIntranetActivity activity);
        void Save(IIntranetActivity activity);
        bool CanEdit(IIntranetActivity cached);
    }


    public interface IIntranetActivityService : ITypedService
    {
        void Delete(Guid id);
        bool CanEdit(Guid id);
    }
}