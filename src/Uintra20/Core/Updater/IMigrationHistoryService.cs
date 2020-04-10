using System;
using System.Collections.Generic;
using Uintra20.Core.Updater.Sql;

namespace Uintra20.Core.Updater
{
    public interface IMigrationHistoryService
    {
        MigrationHistory GetLast();
        List<MigrationHistory> GetAll();
        void Create(string name, Version version);
        void Create(IEnumerable<(string name, Version version)> history);
        bool Exists(string name, Version version);
    }
}
