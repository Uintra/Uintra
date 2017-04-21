using System;
using System.Web.Mvc;
using Compent.uCommunity.Core.Events;
using Compent.uCommunity.Core.News.Entities;
using uCommunity.Core.Activity;
using uCommunity.Events;
using uCommunity.News;

namespace Compent.uCommunity.Core
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
            var type = _activityRepository.Get(id).Type;
            return GetService<TService>(type);
        }

        public TService GetService<TService>(IntranetActivityTypeEnum type) where TService : class
        {
            return (TService)GetService(type);
        }

        public TService GetServiceSafe<TService>(Guid id) where TService : class
        {
            var type = _activityRepository.Get(id).Type;
            return GetServiceSafe<TService>(type);
        }

        public TService GetServiceSafe<TService>(IntranetActivityTypeEnum type) where TService : class
        {
            return GetService(type) as TService;
        }

        private object GetService(IntranetActivityTypeEnum type)
        {
            switch (type)
            {
                case IntranetActivityTypeEnum.News:
                    return _kernel.GetService<INewsService>();
                case IntranetActivityTypeEnum.Events:
                    return _kernel.GetService<IEventsService>();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
