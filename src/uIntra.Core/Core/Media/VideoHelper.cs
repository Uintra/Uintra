using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Extensions;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using uIntra.Core.Constants;
using Umbraco.Core.Models;

namespace uIntra.Core.Media
{
    public class VideoHelper : IVideoHelper
    {
        private List<string> VideoExtensions = new List<string> { "mp4", "avi" };

        private const string ThumbnailFileExtensions = ".jpg";

        public bool IsVideo(string fileExtension)
        {
            return VideoExtensions.Contains(fileExtension.TrimStart('.'));
        }

        public string CreateThumbnail(IMedia media)
        {
            var fileUrl = media.GetValue<string>(UmbracoAliases.Media.UmbracoFilePropertyAlias);
            var fileFullPath = HostingEnvironment.MapPath(fileUrl);

            var directoryName = Path.GetDirectoryName(fileFullPath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileFullPath);
            var outputFileFullPath = Path.Combine(directoryName, $"{fileNameWithoutExtension}{ThumbnailFileExtensions}");

            using (var engine = new Engine())
            {
                var inputMediaFile = new MediaFile { Filename = fileFullPath };
                var outputMediaFile = new MediaFile { Filename = outputFileFullPath };

                engine.GetMetadata(inputMediaFile);

                var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(inputMediaFile.Metadata.Duration.TotalSeconds / 2) };

                engine.GetThumbnail(inputMediaFile, outputMediaFile, options);

                return GetThumbnailUrl(fileUrl);
            }
        }

        public VideoSizeMetadataModel GetSizeMetadata(IMedia media)
        {
            var filePath = media.GetValue<string>(UmbracoAliases.Media.UmbracoFilePropertyAlias);
            var fileFullPath = HostingEnvironment.MapPath(filePath);

            var inputMediaFile = new MediaFile { Filename = fileFullPath };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputMediaFile);

                var sizeMetadata = inputMediaFile.Metadata.VideoData.FrameSize.SplitBy("x").Select(int.Parse).ToArray();

                return new VideoSizeMetadataModel
                {
                    Width = sizeMetadata[0],
                    Height = sizeMetadata[1]
                };
            }
        }

        private static string GetThumbnailUrl(string videoUrl)
        {
            var urlSegments = videoUrl.SplitBy("/").ToList();
            var fileName = urlSegments.Last();
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            urlSegments.Remove(fileName);
            urlSegments.Add($"{fileNameWithoutExtension}{ThumbnailFileExtensions}");

            return $"/{urlSegments.JoinWith("/")}";
        }
    }
}
