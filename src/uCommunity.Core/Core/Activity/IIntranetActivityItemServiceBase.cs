using System;
using System.Collections.Generic;
using uCommunity.Core.Activity.Entities;
using Umbraco.Core.Models;

namespace uCommunity.Core.Activity
{
    public interface IIntranetActivityItemServiceBase<in T, out TModel> : IIntranetActivityItemServiceBase
        where T : IntranetActivityBase
        where TModel : IntranetActivityBase
    {
        TModel Get(Guid id);
        IEnumerable<TModel> GetManyActual();
        IEnumerable<TModel> GetAll(bool includeHidden = false);

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