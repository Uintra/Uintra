using Compent.CommandBus;
using Uintra.Features.Media.Video.Models;

namespace Uintra.Features.Media.Video.Commands
{
    public class VideoConvertedCommand : ICommand
    {
        public bool Success { get; set; }
        public string ConvertedFilePath { get; set; }
        public int MediaId { get; set; }
        public VideoConvertedCommandMessageModel Message { get; set; }
    }
}