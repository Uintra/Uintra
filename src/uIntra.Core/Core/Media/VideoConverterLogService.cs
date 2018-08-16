using System;
using Uintra.Core.Extensions;
using Uintra.Core.Persistence;

namespace Uintra.Core.Media
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
                Date = DateTime.Now
            });
        }
    }
}
