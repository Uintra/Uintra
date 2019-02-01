using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LanguageExt;
using Localization.Core;
using Uintra.CentralFeed;
using static LanguageExt.Prelude;

namespace Compent.Uintra.Core.Updater.Migrations._1._2.Steps
{
    public class SplitEventLabels
    {
        public static TranslationsUpdateStep SplitEventLabelsTranslationsUpdateStep()
        {
            var localizationCoreService = DependencyResolver.Current.GetService<ILocalizationCoreService>();

            var centralFeedTypeProvider = DependencyResolver.Current.GetService<IFeedTypeProvider>();
            const string defaultLang = Migrations._0._0._0._1.Constants.LocalizationConstants.CultureKeys.English;


            Option<(string key, string translation)[]> GetTranslationsByTypes(Enum type) =>
                Optional(localizationCoreService.GetResourceModel(type.ToString()))
                    .Map(resourceModel => resourceModel.Translations[defaultLang])
                    .Map(oldTranslation => new[]
                    {
                        ($"ActivityItemHeader.CategoryName.{type}.lbl", oldTranslation),
                        ($"CentralFeed.Filter.{type}.lnk", oldTranslation)
                    });


            var translations = centralFeedTypeProvider
                .All
                .Select(GetTranslationsByTypes)
                .Somes()
                .SelectMany(identity)
                .ToDictionary(pair => pair.key, pair => pair.translation);

            return new TranslationsUpdateStep(new TranslationUpdateData
            {
                Add = translations,
                Remove = new List<string>()
            });
        }
    }
}