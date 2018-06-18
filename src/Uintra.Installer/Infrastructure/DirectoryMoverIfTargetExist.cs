using System;
using System.Collections.Generic;
using System.IO;

namespace Uintra.Installer.Infrastructure
{
    public class DirectoryMoverIfTargetExist
    {
        private readonly DirectoryInfo _source;
        private readonly DirectoryInfo _target;

        public DirectoryMoverIfTargetExist(DirectoryInfo source, DirectoryInfo target)
        {
            this._source = source;
            this._target = target;
        }

        public bool Move(Action<Exception, bool> logAction)
        {
            try
            {
                if (this._target.Exists)
                {
                    this.MoveFiles((IEnumerable<FileInfo>)this._source.GetFiles(), logAction);
                    this.MoveDirectories((IEnumerable<DirectoryInfo>)this._source.GetDirectories(), logAction);
                    this._source.Delete(true);
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

        private void MoveAndCreate(Action<Exception, bool> logAction)
        {
            this.MoveFiles((IEnumerable<FileInfo>)this._source.GetFiles(), logAction);
            this.MoveDirectories((IEnumerable<DirectoryInfo>)this._source.GetDirectories(), logAction);
        }

        private void MoveFiles(IEnumerable<FileInfo> files, Action<Exception, bool> logAction)
        {
            foreach (FileInfo file in files)
                new FileMover(file, new FileInfo(this._target.FullName + "/" + file.Name)).Move(false, logAction);
        }

        private void MoveDirectories(IEnumerable<DirectoryInfo> directories, Action<Exception, bool> logAction)
        {
            foreach (DirectoryInfo directory in directories)
                new DirectoryMoverIfTargetExist(directory, new DirectoryInfo(this._target.FullName + "/" + directory.Name)).MoveAndCreate(logAction);
        }
    }
}
