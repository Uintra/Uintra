using System.Web.Hosting;
using FluentScheduler;

namespace Uintra.Core.Jobs.Models
{
    public interface IIntranetJob : IJob, IRegisteredObject
    {        
        void Action();     
    }
}
