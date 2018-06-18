using System;
using System.IO;

namespace Uintra.Installer.Infrastructure
{
    public class DirectoryMover
    {
        private readonly DirectoryInfo _source;
        private readonly DirectoryInfo _target;
        private readonly bool _overwriteTarget;

        public DirectoryMover(DirectoryInfo source, DirectoryInfo target, bool overwriteTarget)
        {
            this._source = source;
            this._target = target;
            this._overwriteTarget = overwriteTarget;
        }

        public bool Move(Action<Exception, bool> logAction)
        {
            try
            {
                if (this._overwriteTarget && this._target.Exists)
                    this._target.Delete(true);
                this._source.MoveTo(this._target.FullName);
                logAction(null, true);
            }
            catch (Exception ex)
            {
                logAction(ex, false);
                return false;
            }
            return true;
        }
    }
}
