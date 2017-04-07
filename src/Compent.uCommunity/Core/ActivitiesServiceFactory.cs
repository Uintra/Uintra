using System;
using System.Web.Mvc;
using uCommunity.Core.Activity;
using uCommunity.News;

namespace Compent.uCommunity.Core
{
    public class ActivitiesServiceFactory : IActivitiesServiceFactory
    {
        private readonly IDependencyResolver _kernel;
        private readonly IIntranetActivityService _intranetActivityService;

        public ActivitiesServiceFactory(
            IDependencyResolver kernel,
            IIntranetActivityService intranetActivityService)
        {
            _kernel = kernel;
            _intranetActivityService = intranetActivityService;
        }

        public IIntranetActivityItemServiceBase GetService(IntranetActivityTypeEnum type)
        {
            switch (type)
            {
                case IntranetActivityTypeEnum.News:
                    return _kernel.GetService<INewsService<NewsBase, NewsModelBase>>();
                //case IntranetActivityTypeEnum.Events:
                //    return _kernel.GetService<IEventsService<EventBase, EventModelBase>>();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IIntranetActivityItemServiceBase GetService(Guid id)
        {
            var type = _intranetActivityService.GetType(id);
            return GetService(type);
        }
    }
}
