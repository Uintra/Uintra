using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LightInject;
using UBaseline.Core.Extensions;
using Uintra20.Features.Media.Constants;
using Uintra20.Features.Media.Enums;
using Uintra20.Infrastructure.Constants;
using Umbraco.Core.Composing;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;

namespace Uintra20.Core.Updater._2._0.Steps
{
    public class SetupDefaultMediaFoldersStep : IMigrationStep
    {
        private readonly ILogger _logger;
        private readonly IMediaService _mediaService;

        public SetupDefaultMediaFoldersStep()
        {
            _logger = Current.Factory.EnsureScope(f => f.GetInstance<ILogger>());
            _mediaService=Current.Factory.EnsureScope(f => f.GetInstance<IMediaService>());
        }

        public ExecutionResult Execute()
        {        
           _logger.Info<SetupDefaultMediaFoldersStep>("SetupDefaultMediaFoldersStep is running");
           CreateDefaultFolders();
           return ExecutionResult.Success;
        }
        
        public void Undo()
        {
        }
        
        private void CreateDefaultFolders()
        {
            var folderTypes = Enum.GetValues(typeof(MediaFolderTypeEnum)).Cast<MediaFolderTypeEnum>();
            var rootFolders = _mediaService.GetRootMedia().Where(f => f.ContentType.Alias.Equals(UmbracoAliases.Media.FolderTypeAlias));
            foreach (var folderType in folderTypes)
            {
                var folderName =Infrastructure.Extensions.EnumExtensions.GetAttribute<DisplayAttribute>(folderType).Name;
                var folderByName = rootFolders.Where(m => m.Name.Equals(folderName));
                var folderByType = rootFolders.Where(m => m.GetValue<MediaFolderTypeEnum>(FolderConstants.FolderTypePropertyTypeAlias) == folderType);

                if (folderByName.Any() || folderByType.Any())
                {
                    _logger.Info<SetupDefaultMediaFoldersStep>($"<{folderType.ToString()}> is existed");
                    continue;
                }

                var mediaFolder = _mediaService.CreateMedia(folderName, -1, UmbracoAliases.Media.FolderTypeAlias);
                mediaFolder.SetValue(FolderConstants.FolderTypePropertyTypeAlias, folderType.ToString());
                Umbraco.Web.Composing.Current.Factory.EnsureScope(s => { _mediaService.Save(mediaFolder); });
                _logger.Info<SetupDefaultMediaFoldersStep>($"<{folderType.ToString()}> folder created");
            }
        }
    }
}