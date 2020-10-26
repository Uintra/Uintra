using System;
using System.Threading.Tasks;
using UBaseline.Core.Extensions;
using Uintra.Features.Media.Video.Entities;
using Uintra.Features.Media.Video.Services.Contracts;
using Uintra.Persistence.Sql;

namespace Uintra.Features.Media.Video.Services.Implementations
{
    public class VideoConverterLogService : IVideoConverterLogService
    {
        private readonly ISqlRepository<Guid, VideoConvertationLog> _sqlLogRepository;

        public VideoConverterLogService(ISqlRepository<Guid, VideoConvertationLog> sqlLogRepository) =>
            _sqlLogRepository = sqlLogRepository;

        public void Log(bool result, string message, int mediaId) =>
            _sqlLogRepository.Add(CreateLoggingEntity(result, message, mediaId));

        public async Task LogAsync(bool result, string message, int mediaId) =>
            await _sqlLogRepository.AddAsync(CreateLoggingEntity(result, message, mediaId));

        public VideoConvertationLog CreateLoggingEntity(
            bool result,
            string message,
            int mediaId)
        {
            return new VideoConvertationLog
            {
                Id = Guid.NewGuid(),
                Message = message.ToJson(),
                MediaId = mediaId,
                Result = result,
                Date = DateTime.UtcNow
            };
        }
    }
}