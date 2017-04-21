using System;
using System.Collections.Generic;
using uCommunity.Core.Activity.Entities;
using Umbraco.Core.Models;

namespace uCommunity.Core.Activity
{
    public interface IIntranetActivityService<TCachedActivity>: IIntranetActivityService where TCachedActivity : IntranetActivity
    {
        IntranetActivityTypeEnum ActivityType { get; }

        TCachedActivity Get(Guid id);
        IEnumerable<TCachedActivity> GetManyActual();
        IEnumerable<TCachedActivity> FindAll(Func<TCachedActivity, bool> predicate);
        IEnumerable<TCachedActivity> GetAll(bool includeHidden = false);
        bool IsActual(TCachedActivity cachedActivity);
        Guid Create(TCachedActivity jsonData);
        void Save(TCachedActivity saveModel);
        bool CanEdit(TCachedActivity cached);
    }

    public interface IIntranetActivityService
    {
        void Delete(Guid id);
        bool CanEdit(Guid id);

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