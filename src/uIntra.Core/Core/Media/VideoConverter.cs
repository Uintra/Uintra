using System;
using System.IO;
using System.Threading;
using Compent.CommandBus;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using Uintra.Core.Constants;

namespace Uintra.Core.Media
{
    public class VideoConverter : IVideoConverter
    {
        private const string Mp4ExtensionName = ".mp4";
        private readonly ICommandPublisher _commandPublisher;
        private readonly Engine _engine;

        public VideoConverter(ICommandPublisher commandPublisher)
        {
            _commandPublisher = commandPublisher;
            _engine = new Engine();
        }

        public void Convert(MediaConvertModel model)
        {            
            VideoConvertedCommand command = null;
            try
            {
                var tempFilePath = Path.GetTempFileName();
                File.WriteAllBytes(tempFilePath, model.File.FileBytes);

                var inputFile = new MediaFile { Filename = tempFilePath };
                var outputFile = new MediaFile { Filename = Path.ChangeExtension(tempFilePath, Mp4ExtensionName) };

                using (_engine)
                {
                    _engine.GetMetadata(inputFile);
                    var options = new ConversionOptions();
                    _engine.Convert(inputFile, outputFile, options);
                }

                File.Delete(inputFile.Filename);

                command = new VideoConvertedCommand
                {
                    Success = true,
                    ConvertedFilePath = outputFile.Filename,
                    MediaId = model.MediaId
                };

            }
            catch (Exception ex)
            {
                command = new VideoConvertedCommand()
                {
                    MediaId = model.MediaId,
                    Success = false,
                    Message = new VideoConvertedCommandMessageModel()
                    {
                        ExceptionMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        FileName = model.File.FileName
                    }
                };
            }
            finally
            {
                _commandPublisher.Publish(command);
            }

        }

        public bool NeedConvert(string mediaTypeAlias, string fileName)
        {
            return mediaTypeAlias == UmbracoAliases.Media.VideoTypeAlias && Path.GetExtension(fileName) != Mp4ExtensionName;
        }

    }

    public class VideoConvertedCommandMessageModel
    {
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public string FileName { get; set; }
    }
}
