using System.Web.Hosting;

namespace uIntra.Core.Jobs
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
            lock (_lock)
            {
                if (_shuttingDown)
                    return;

                Action();
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


        public virtual JobSettings GetSettings()
        {
            //TODO: in child classes add getting settings from configuration/db/cache...
            return new JobSettings()
            {
                TimeType = JobTimeType.Minutes,
                Time = 1,
                RunType = JobRunTypeEnum.RunEvery,
                IsEnabled = true
            };
        }

        public virtual void Action()
        {

        }
    }
}
