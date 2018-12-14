using System;
using System.Collections.Generic;

namespace Compent.Uintra.Core.Updater.Migrations._1._2
{
    public class Migration : IMigration
    {
        private readonly IMigrationStepsResolver _migrationStepsResolver;

        public Version Version => new Version("1.2");

        public Migration(IMigrationStepsResolver migrationStepsResolver)
        {
            _migrationStepsResolver = migrationStepsResolver;
        }

        private TranslationUpdateData TranslationUpdateData { get; } = new TranslationUpdateData
        {
            Add = new Dictionary<string, string>
            {
                { "Login.VersionIdentificatorPrefix.lbl","v"},
                { "TopNavigation.UintraDocumentationLink.lnk","Uintra Help"}
            },
            Remove = new List<string>()
        };

        private T Resolve<T>() where T : class => _migrationStepsResolver.Resolve<T>();

        public IEnumerable<IMigrationStep> Steps
        {
            get
            {
                yield return new TranslationsUpdateStep(TranslationUpdateData);
            }
        }
    }
}