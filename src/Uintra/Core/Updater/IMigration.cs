using System;
using System.Collections.Generic;

namespace Uintra.Core.Updater
{
    public interface IMigration
    {
        Version Version { get; }
        IEnumerable<IMigrationStep> Steps { get; }
    }
}
