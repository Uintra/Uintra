using System.Web.Hosting;

namespace Uintra20.Core.Jobs.Models
{
    public class BaseIntranetJob : IIntranetJob
    {
        private readonly object _lock = new object();

        private bool _shuttingDown;

        public BaseIntranetJob()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public void Execute()
        {
            try
            {
                lock (_lock)
                {
                    if (_shuttingDown)
                        return;
                    Action();
                }
            }
            finally
            {             
                HostingEnvironment.UnregisterObject(this);
            }
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }

        public virtual void Action()
        {

        }
    }
}
