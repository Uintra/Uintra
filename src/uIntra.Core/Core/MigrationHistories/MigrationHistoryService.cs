using System;
using System.Linq;
using uIntra.Core.MigrationHistories.Sql;
using uIntra.Core.Persistence;

namespace uIntra.Core.MigrationHistories
{
    public class MigrationHistoryService : IMigrationHistoryService
    {

        private readonly ISqlRepository<int, MigrationHistory> _migrationHistoryRepository;

        public MigrationHistoryService(ISqlRepository<int, MigrationHistory> migrationHistoryRepository)
        {
            _migrationHistoryRepository = migrationHistoryRepository;
        }

        public MigrationHistory GetLast() => _migrationHistoryRepository
            .GetAll()
            .OrderBy(m => m.Version)
            .ThenBy(m => m.CreateDate)
            .FirstOrDefault();

        public void Create(string name, Version version)
        {
            var migrationHistory = new MigrationHistory
            {
                Name = name,
                Version = version.ToString(),
                CreateDate = DateTime.Now
            };

            _migrationHistoryRepository.Add(migrationHistory);
        }
    }
}
