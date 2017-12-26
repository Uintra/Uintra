using System.Web.Hosting;
using FluentScheduler;

namespace uIntra.Core.Jobs.Models
{
    public interface IIntranetJob : IJob, IRegisteredObject
    {        
        void Action();     
    }
}
