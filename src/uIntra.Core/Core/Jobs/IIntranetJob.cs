using System.Web.Hosting;
using FluentScheduler;

namespace uIntra.Core.Jobs
{
    public interface IIntranetJob : IJob, IRegisteredObject
    {        
        void Action();

        JobSettings GetSettings();
    }
}
