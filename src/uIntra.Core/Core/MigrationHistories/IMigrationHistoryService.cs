using uIntra.Core.MigrationHistories.Sql;

namespace uIntra.Core.MigrationHistories
{
    public interface IMigrationHistoryService
    {
        MigrationHistory GetLast();
        void Create(string version);
    }
}
