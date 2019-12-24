using System.Web.Mvc;
using FluentScheduler;

namespace Uintra20.Core.Jobs
{
    public class IntranetJobFactory : IJobFactory
    {
        public IJob GetJobInstance<T>() where T : IJob
        {
            var job = DependencyResolver.Current.GetService<T>();
            return job;
        }
    }
}
