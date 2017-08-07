using System;
using System.Linq;
using uIntra.Core.MigrationHistories.Sql;
using uIntra.Core.Persistence;

namespace uIntra.Core.MigrationHistories
{
    public class MigrationHistoryService : IMigrationHistoryService
    {
        private const string MigrationHistoryName = "uIntra";

        private readonly ISqlRepository<int, MigrationHistory> _migrationHistoryRepository;

        public MigrationHistoryService(ISqlRepository<int, MigrationHistory> migrationHistoryRepository)
        {
            _migrationHistoryRepository = migrationHistoryRepository;
        }

        public MigrationHistory GetLast()
        {
            return _migrationHistoryRepository.GetAll().OrderByDescending(mh => new Version(mh.Version)).FirstOrDefault();
        }

        public void Create(string version)
        {
            var migrationHistory = new MigrationHistory
            {
                Name = MigrationHistoryName,
                Version = version,
                CreateDate = DateTime.Now
            };

            _migrationHistoryRepository.Add(migrationHistory);
        }
    }
}
