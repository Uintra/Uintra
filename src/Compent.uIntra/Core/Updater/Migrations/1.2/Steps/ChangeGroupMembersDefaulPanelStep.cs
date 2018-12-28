using Compent.Uintra.Core.Constants;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Uintra.Core.Utils;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace Compent.Uintra.Core.Updater.Migrations._1._2.Steps
{
    public class ChangeGroupMembersDefaulPanelStep : IMigrationStep
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IContentService _contentService;

        public ChangeGroupMembersDefaulPanelStep(IContentTypeService contentTypeService, IContentService contentService)
        {
            _contentTypeService = contentTypeService;
            _contentService = contentService;
        }

        public ExecutionResult Execute()
        {
            var groupMembersContentType = _contentTypeService.GetContentType(DocumentTypeAliasConstants.GroupsMembersPage);
            var groupMembersPage = _contentService.GetContentOfContentType(groupMembersContentType.Id).First();

            var gridContent = EmbeddedResourcesUtils.ReadResourceContent($"{Assembly.GetExecutingAssembly().GetName().Name}.Core.Updater.Migrations._1._2.PreValues.groupsMembersPageGrid.json");
            groupMembersPage.SetValue(UmbracoContentMigrationConstants.Grid.GridPropName, gridContent);

            _contentService.SaveAndPublishWithStatus(groupMembersPage);
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            
        }
    }
}