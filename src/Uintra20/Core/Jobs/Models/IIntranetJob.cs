using System.Web.Hosting;
using FluentScheduler;

namespace Uintra20.Core.Jobs.Models
{
    public interface IIntranetJob : IJob, IRegisteredObject
    {        
        void Action();     
    }
}
