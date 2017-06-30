using System;
using System.Web.Mvc;
using Compent.uIntra.Core.Bulletins;
using Compent.uIntra.Core.Events;
using uIntra.Bulletins;
using uIntra.Core.Activity;
using uIntra.Events;
using uIntra.News;

namespace Compent.uIntra.Core
{
    public class ActivitiesServiceFactory : IActivitiesServiceFactory
    {
        private readonly IDependencyResolver _kernel;
        private readonly IIntranetActivityRepository _activityRepository;

        public ActivitiesServiceFactory(
            IDependencyResolver kernel,
            IIntranetActivityRepository activityRepository)
        {
            _kernel = kernel;
            _activityRepository = activityRepository;
        }

        public TService GetService<TService>(Guid id) where TService : class
        {
            var repository = _activityRepository.Get(id);
            return repository != null ? GetService<TService>(repository.Type) : null;
        }
        

        public TService GetServiceSafe<TService>(Guid id) where TService : class
        {
            var repository = _activityRepository.Get(id);
            return repository != null ? GetServiceSafe<TService>(repository.Type) : null;
        }
        

        private object GetService(int acitivityTypeId)
        {
            switch (acitivityTypeId)
            {
                case (int)IntranetActivityTypeEnum.News:
                    return _kernel.GetService<INewsService<News.Entities.News>>();
                case (int)IntranetActivityTypeEnum.Events:
                    return _kernel.GetService<IEventsService<Event>>();
                case (int)IntranetActivityTypeEnum.Bulletins:
                    return _kernel.GetService<IBulletinsService<Bulletin>>();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TService GetService<TService>(int acitivityTypeId) where TService : class
        {
            return (TService)GetService(acitivityTypeId);
        }

        public TService GetServiceSafe<TService>(int acitivityTypeId) where TService : class
        {
            return GetService(acitivityTypeId) as TService;
        }
    }
}
