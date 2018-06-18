using System;
using System.Collections.Generic;

namespace Uintra.Core.Activity
{
    public interface IIntranetActivityService<out TActivity> : IIntranetActivityService where TActivity : IIntranetActivity
    {
        TActivity Get(Guid id);
        IEnumerable<TActivity> GetManyActual();
        IEnumerable<TActivity> GetAll(bool includeHidden = false);
        bool IsActual(IIntranetActivity activity);
        bool IsPinActual(IIntranetActivity activity);
        Guid Create(IIntranetActivity activity);
        void Save(IIntranetActivity activity);
        bool CanEdit(IIntranetActivity activity);
    }

    public interface IIntranetActivityService : ITypedService
    {
        void Delete(Guid id);
        bool CanEdit(Guid id);
    }
}