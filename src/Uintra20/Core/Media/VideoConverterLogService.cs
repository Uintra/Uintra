using System;
using System.Threading.Tasks;
using Uintra20.Core.Extensions;
using Uintra20.Persistence;

namespace Uintra20.Core.Media
{
    public class VideoConverterLogService : IVideoConverterLogService
    {
        private readonly ISqlRepository<Guid, VideoConvertationLog> _sqlLogRepository;

        public VideoConverterLogService(ISqlRepository<Guid, VideoConvertationLog> sqlLogRepository)
        {
            _sqlLogRepository = sqlLogRepository;
        }

        public void Log(bool result, string message, int mediaId)
        {
            _sqlLogRepository.Add(new VideoConvertationLog
            {
                Id = Guid.NewGuid(),
                Message = message.ToJson(),
                MediaId = mediaId,
                Result = result,
                Date = DateTime.UtcNow
            });
        }

        public async Task LogAsync(bool result, string message, int mediaId)
        {
            await _sqlLogRepository.AddAsync(new VideoConvertationLog
            {
                Id = Guid.NewGuid(),
                Message = message.ToJson(),
                MediaId = mediaId,
                Result = result,
                Date = DateTime.UtcNow
            });
        }
    }
}