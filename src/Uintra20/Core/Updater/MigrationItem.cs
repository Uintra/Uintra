using System;

namespace Uintra20.Core.Updater
{
    public struct MigrationItem
    {
        public Version Version { get; }
        public IMigrationStep Step { get; }

        public MigrationItem(Version version, IMigrationStep step)
        {
            Version = version;
            Step = step;
        }
    }
}