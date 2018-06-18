using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Extensions;
using Localization.Core;
using uIntra.Core.Extensions;
using uIntra.Core.Utils;

namespace Compent.uIntra.Core.Updater.Migrations._0._0._0._1.Steps.AggregateSubsteps
{
    public class DefaultLocalizationsMigration
    {
        private readonly string DefaultLocalizationsEmbeddedResourceFilePath = $"{Assembly.GetExecutingAssembly().GetName().Name}.Core.Updater.Migrations._0._0._0._1.PreValues.DefaultLocalizations.json";

        private readonly ILocalizationCoreService _localizationCoreService;

        public DefaultLocalizationsMigration()
        {
            _localizationCoreService = HttpContext.Current.GetService<ILocalizationCoreService>();
        }

        public void Init()
        {
            AddLocalizations();
        }

        private void AddLocalizations()
        {
            var existedLocalizations = _localizationCoreService.GetAllResourceModels();

            var fileContent = EmbeddedResourcesUtils.ReadResourceContent(DefaultLocalizationsEmbeddedResourceFilePath, Assembly.GetExecutingAssembly());
            var newLocalizations = fileContent.Deserialize<List<ResourceModel>>();

            var parentKeys = newLocalizations
                .Where(loc => loc.ParentKey.HasValue())
                .Select(loc => loc.ParentKey)
                .Distinct();

            var parentLocalizations = newLocalizations.Where(loc => parentKeys.Contains(loc.Key)).ToList();

            AddNewLocalizationsIfNotExists(existedLocalizations, parentLocalizations);
            AddNewLocalizationsIfNotExists(existedLocalizations, newLocalizations.Except(parentLocalizations));
        }

        private void AddNewLocalizationsIfNotExists(List<ResourceModel> existedLocalizations, IEnumerable<ResourceModel> newLocalizations)
        {
            foreach (var loc in newLocalizations)
            {
                if (existedLocalizations.Exists(el => el.Key.Equals(loc.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                _localizationCoreService.Create(loc);
            }
        }
    }
}