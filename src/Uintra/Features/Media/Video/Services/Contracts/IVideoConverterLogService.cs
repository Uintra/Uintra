﻿using System.Threading.Tasks;

namespace Uintra.Features.Media.Video.Services.Contracts
{
    public interface IVideoConverterLogService
    {
        void Log(bool result, string message, int mediaId);
        Task LogAsync(bool result, string message, int mediaId);
    }
}