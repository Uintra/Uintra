using System;
using System.Collections.Generic;
using uCommunity.Core.Activity.Entities;
using Umbraco.Core.Models;

namespace uCommunity.Core.Activity
{
    public interface IIntranetActivityService<in TCachedActivity>: IIntranetActivityService where TCachedActivity : IntranetActivity
    {
        IntranetActivityTypeEnum ActivityType { get; }

        TActivity Get<TActivity>(Guid id) where TActivity: TCachedActivity;
        IEnumerable<TActivity> GetManyActual<TActivity>() where TActivity : TCachedActivity;
        IEnumerable<TActivity> FindAll<TActivity>(Func<TActivity, bool> predicate) where TActivity : TCachedActivity;
        IEnumerable<TActivity> GetAll<TActivity>(bool includeHidden = false) where TActivity : TCachedActivity;
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