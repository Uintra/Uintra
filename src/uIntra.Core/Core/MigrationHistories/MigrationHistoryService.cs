using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.MigrationHistories.Sql;
using Uintra.Core.Persistence;

namespace Uintra.Core.MigrationHistories
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
            .OrderByDescending(m => m.CreateDate)
            .FirstOrDefault();

        public List<MigrationHistory> GetAll() => _migrationHistoryRepository
            .GetAll()
            .ToList();

        public void Create(string name, Version version)
        {
            var migrationHistory = new MigrationHistory
            {
                Name = name,
                Version = version.ToString(),
                CreateDate = DateTime.UtcNow
            };

            _migrationHistoryRepository.Add(migrationHistory);
        }

        public void Create(IEnumerable<(string name, Version version)> history)
        {
            var migrationHistory = history.Select(h => new MigrationHistory
            {
                Name = h.name,
                Version = h.version.ToString(),
                CreateDate = DateTime.UtcNow
            });
            _migrationHistoryRepository.Add(migrationHistory);
        }
    }
}
