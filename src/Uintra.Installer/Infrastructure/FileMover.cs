using System;
using System.IO;

namespace Uintra.Installer.Infrastructure
{
    public class FileMover
    {
        private readonly FileInfo _sourceFilename;
        private readonly FileInfo _targetFilename;

        public FileMover(FileInfo sourceFilename, FileInfo targetFilename)
        {
            this._sourceFilename = sourceFilename;
            this._targetFilename = targetFilename;
        }

        public bool Move(bool backupTarget, Action<Exception, bool> logAction)
        {
            try
            {
                if (!this._targetFilename.Directory.Exists)
                    this._targetFilename.Directory.Create();
                if (this._sourceFilename.Exists)
                {
                    if (backupTarget && this._targetFilename.Exists)
                        this._targetFilename.CopyTo(string.Format(this._targetFilename.FullName + ".{0}.backup", DateTime.Now.Ticks));
                    if (File.Exists(this._targetFilename.FullName))
                        File.Delete(this._targetFilename.FullName);
                    this._sourceFilename.MoveTo(this._targetFilename.FullName);
                }
                logAction(null, true);
            }
            catch (Exception ex)
            {
                logAction(ex, false);
                return false;
            }
            return true;
        }

        public bool MoveIfDoesntExist(Action<Exception, bool> logAction)
        {
            try
            {
                if (!this._targetFilename.Exists)
                    this._sourceFilename.MoveTo(this._targetFilename.FullName);
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
