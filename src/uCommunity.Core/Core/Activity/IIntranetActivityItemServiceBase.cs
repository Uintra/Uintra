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
        TModel FillCache(Guid id);
        void Delete(Guid id);
    }

    public interface IIntranetActivityItemServiceBase
    {
        [Obsolete("Use overloading method instead")]
        IPublishedContent GetOverviewPage();
        [Obsolete("Use overloading method instead")]
        IPublishedContent GetDetailsPage();
        [Obsolete("Use overloading method instead")]
        IPublishedContent GetCreatePage();
        [Obsolete("Use overloading method instead")]
        IPublishedContent GetEditPage();

        IPublishedContent GetOverviewPage(IPublishedContent currentPage);
        IPublishedContent GetDetailsPage(IPublishedContent currentPage);
        IPublishedContent GetCreatePage(IPublishedContent currentPage);
        IPublishedContent GetEditPage(IPublishedContent currentPage);
    }
}