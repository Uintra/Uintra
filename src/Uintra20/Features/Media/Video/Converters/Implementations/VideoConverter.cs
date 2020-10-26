using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Compent.CommandBus;
using Uintra20.Core.MediaToolkit;
using Uintra20.Core.MediaToolkit.Model;
using Uintra20.Features.Media.Models;
using Uintra20.Features.Media.Video.Commands;
using Uintra20.Features.Media.Video.Converters.Contracts;
using Uintra20.Features.Media.Video.Models;
using Uintra20.Infrastructure.ApplicationSettings;
using Uintra20.Infrastructure.Constants;
using Umbraco.Core.Models;
using File = System.IO.File;

namespace Uintra20.Features.Media.Video.Converters.Implementations
{
    public class VideoConverter : IVideoConverter
    {
        private const string Mp4ExtensionName = ".mp4";
        private readonly ICommandPublisher _commandPublisher;
        private readonly IApplicationSettings _applicationSettings;

        public VideoConverter(
            ICommandPublisher commandPublisher, 
            IApplicationSettings applicationSettings)
        {
            _commandPublisher = commandPublisher;
            _applicationSettings = applicationSettings;
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

                using (var engine=new Engine(HostingEnvironment.MapPath(IntranetConstants.FfmpegRelativePath)))
                {
                    engine.GetMetadata(inputFile);
                    var options = new Uintra20.Core.MediaToolkit.Options.ConversionOptions();
                    engine.Convert(inputFile, outputFile, options);
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
                command = new VideoConvertedCommand
                {
                    MediaId = model.MediaId,
                    Success = false,
                    Message = new VideoConvertedCommandMessageModel
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
            var ext = Path.GetExtension(filename)?.Replace(".", string.Empty); ;
            return _applicationSettings.VideoFileTypes.Contains(ext);
        }
    }
}