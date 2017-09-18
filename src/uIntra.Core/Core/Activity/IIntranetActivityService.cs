using System;
using System.Collections.Generic;
using uIntra.Core.TypeProviders;

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


    public interface IIntranetActivityService
    {
        IIntranetType ActivityType { get; }
        void Delete(Guid id);
        bool CanEdit(Guid id);
    }

}