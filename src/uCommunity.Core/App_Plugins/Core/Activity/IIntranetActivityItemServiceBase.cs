using System;
using System.Collections.Generic;
using uCommunity.Core.Activity.Entities;
using Umbraco.Core.Models;

namespace uCommunity.Core.Activity
{
    public interface IIntranetActivityItemServiceBase<T> : IIntranetActivityItemServiceBase
        where T : IntranetActivityBase
    {
        T Get(Guid id);
        IEnumerable<T> GetManyActual();
        IEnumerable<T> GetAll(bool includeHidden = false);
        bool IsActual(T activity);
        bool CanEdit(T activity);
        Guid Create(T model);
        void Save(T model);
    }

    public interface IIntranetActivityItemServiceBase
    {
        IPublishedContent GetOverviewPage();
        IPublishedContent GetDetailsPage();
        IPublishedContent GetCreatePage();
        IPublishedContent GetEditPage();
        void Delete(Guid id);
        void FillCache(Guid id);
    }
}