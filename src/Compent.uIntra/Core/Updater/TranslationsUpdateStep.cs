using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Extensions;
using Compent.Uintra.Core.Updater.Migrations._0._0._0._1;

namespace Compent.Uintra.Core.Updater
{
    public class TranslationsUpdateStep : IMigrationStep
    {
        private readonly TranslationUpdateData translationData;

        public TranslationsUpdateStep(TranslationUpdateData translationData)
        {
            this.translationData = translationData;
        }

        public ExecutionResult Execute()
        {
            foreach (var (key, translation) in translationData.Add)
            {
                InstallationStepsHelper.AddTranslation(key, translation);
            }

            foreach (var (key, (old, update)) in translationData.Update)
            {
                InstallationStepsHelper.UpdateTranslation(key, old, update);
            }

            foreach (var removeItem in translationData.Remove)
            {
                InstallationStepsHelper.DeleteTranslation(removeItem);
            }

            return ExecutionResult.Success;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }


    public struct TranslationUpdateData
    {
        public Dictionary<string, string> Add { get; set; }
        public Dictionary<string, (string old, string update)> Update { get; set; }
        public List<string> Remove { get; set; }
    }
}