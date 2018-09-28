using Compent.CommandBus;

namespace Uintra.Core.Media
{
    public class VideoConvertedCommand : ICommand
    {
        public bool Success { get; set; }
        public string ConvertedFilePath { get; set; }
        public int MediaId { get; set; }        
        public VideoConvertedCommandMessageModel Message { get; set; }
    }
}
