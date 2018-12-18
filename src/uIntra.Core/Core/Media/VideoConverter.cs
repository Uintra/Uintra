using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Compent.CommandBus;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Constants;
using Umbraco.Core.Models;
using File = System.IO.File;

namespace Uintra.Core.Media
{
    public class VideoConverter : IVideoConverter
    {
        private const string Mp4ExtensionName = ".mp4";
        private readonly ICommandPublisher _commandPublisher;
        private readonly IApplicationSettings _applicationSettings;
        private readonly Engine _engine;
        private readonly string _ffmpegPath = HostingEnvironment.MapPath(IntranetConstants.FfmpegRelativePath);

        public VideoConverter(ICommandPublisher commandPublisher, IApplicationSettings applicationSettings)
        {
            _commandPublisher = commandPublisher;
            _applicationSettings = applicationSettings;
            _engine = new Engine(_ffmpegPath);
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
            return mediaTypeAlias == UmbracoAliases.Media.VideoTypeAlias && !IsMp4(fileName);
        }     

        public bool IsConverting(IMedia media)
        {
           return media.GetValue<bool>(UmbracoAliases.Video.ConvertInProcessPropertyAlias);
        }

        public bool IsMp4(string filename)
        {
            return Path.GetExtension(filename) == Mp4ExtensionName;
        }

        public bool IsVideo(string filename)
        {
            var ext=Path.GetExtension(filename)?.Replace(".", ""); ;
            return _applicationSettings.VideoFileTypes.Contains(ext);
        }
    }

    public class VideoConvertedCommandMessageModel
    {
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public string FileName { get; set; }
    }
}
