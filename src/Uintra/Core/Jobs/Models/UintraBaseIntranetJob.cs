﻿using System.Web.Hosting;

namespace Uintra.Core.Jobs.Models
{
    public class UintraBaseIntranetJob : IIntranetJob
    {
        private readonly object _lock = new object();

        private bool _shuttingDown;

        public UintraBaseIntranetJob()
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
