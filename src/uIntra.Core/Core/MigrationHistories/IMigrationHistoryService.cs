using System;
using System.Collections.Generic;
using uIntra.Core.MigrationHistories.Sql;

namespace uIntra.Core.MigrationHistories
{
    public interface IMigrationHistoryService
    {
        MigrationHistory GetLast();
        List<MigrationHistory> GetAll();
        void Create(string name, Version version);
        void Create(IEnumerable<(string name, Version version)> history);
    }
}
