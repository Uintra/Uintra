using Compent.CommandBus;
using Uintra20.Features.Media.Video.Models;

namespace Uintra20.Features.Media.Video.Commands
{
    public class VideoConvertedCommand : ICommand
    {
        public bool Success { get; set; }
        public string ConvertedFilePath { get; set; }
        public int MediaId { get; set; }
        public VideoConvertedCommandMessageModel Message { get; set; }
    }
}