using System;
using System.Collections.Generic;

namespace Uintra20.Core.Updater
{
    public interface IMigration
    {
        Version Version { get; }
        IEnumerable<IMigrationStep> Steps { get; }
    }
}
