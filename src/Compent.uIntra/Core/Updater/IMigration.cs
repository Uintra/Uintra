using System;
using System.Collections.Generic;

namespace Compent.uIntra.Core.Updater
{
    public interface IMigration
    {
        Version Version { get; }
        IEnumerable<IMigrationStep> Steps { get; }
    }
}
