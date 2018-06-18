using System;
using System.Globalization;
using System.IO;

namespace Uintra.Installer.Infrastructure
{
    public class FileCopier
    {
        private readonly string _sourceFilename;
        private readonly string _targetFilename;

        public FileCopier(FileInfo sourceFilename, FileInfo targetFilename)
        {
            this._sourceFilename = sourceFilename.FullName;
            this._targetFilename = targetFilename.FullName.Replace("{DateTime.Now.Ticks}", DateTime.Now.Ticks.ToString((IFormatProvider)CultureInfo.InvariantCulture));
        }

        public bool Copy(Action<Exception> logAction)
        {
            try
            {
                if (this._sourceFilename != null)
                {
                    if (this._targetFilename != null)
                    {
                        if (!File.Exists(this._targetFilename))
                            File.Copy(this._sourceFilename, this._targetFilename);
                    }
                }
            }
            catch (Exception ex)
            {
                logAction(ex);
                return false;
            }
            return true;
        }
    }
}
