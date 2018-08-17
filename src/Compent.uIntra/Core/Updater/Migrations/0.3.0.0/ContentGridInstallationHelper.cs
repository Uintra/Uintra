using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Umbraco.Core.Models;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0
{
    public static class GridInstallationHelper
    {
        private const int OneColumnRowIndex = 0;
        private const int TwoColumnRowIndex = 1;
        private const int MainRowIndex = 0;
        private const int RightSideColumnIndex = 1;

        public static void AddAllowedEditorForOneColumnRow(IDictionary<string, PreValue> preValues, string allowedEditorAlias)
        {
            EditAllowedEditor(preValues, allowedEditorAlias, OneColumnRowIndex);
        }

        public static void AddAllowedEditorForTwoColumnRow(IDictionary<string, PreValue> preValues, string allowedEditorAlias)
        {
            EditAllowedEditor(preValues, allowedEditorAlias, TwoColumnRowIndex);
        }

        public static void RemoveAllowedEditorForOneColumnRow(IDictionary<string, PreValue> preValues, string allowedEditorAlias)
        {
            EditAllowedEditor(preValues, allowedEditorAlias, OneColumnRowIndex, deleteEditor: true);
        }

        public static void RemoveAllowedEditorForTwoColumnRow(IDictionary<string, PreValue> preValues, string allowedEditorAlias)
        {
            EditAllowedEditor(preValues, allowedEditorAlias, TwoColumnRowIndex, deleteEditor: true);
        }

        public static void AddAllowedEditorForMainRow(IDictionary<string, PreValue> preValues, string allowedEditorAlias)
        {
            EditAllowedEditor(preValues, allowedEditorAlias, MainRowIndex);
        }

        public static void AddAllowedEditorForRightSideColumnRow(IDictionary<string, PreValue> preValues, string allowedEditorAlias)
        {
            EditAllowedEditor(preValues, allowedEditorAlias, RightSideColumnIndex);
        }

        public static void RemoveAllowedEditorForMainRow(IDictionary<string, PreValue> preValues, string allowedEditorAlias)
        {
            EditAllowedEditor(preValues, allowedEditorAlias, MainRowIndex, deleteEditor: true);
        }

        public static void RemoveAllowedEditorForRightSideColumnRow(IDictionary<string, PreValue> preValues, string allowedEditorAlias)
        {
            EditAllowedEditor(preValues, allowedEditorAlias, RightSideColumnIndex, deleteEditor: true);
        }

        private static void EditAllowedEditor(IDictionary<string, PreValue> preValues, string allowedEditorAlias, int layoutIndex, bool deleteEditor = false)
        {
            foreach (var preValueItem in preValues)
            {
                var parsedPreValue = JObject.Parse(preValueItem.Value.Value);

                var areaTokens = parsedPreValue.SelectTokens($"layouts[{layoutIndex}].areas[*]");
                foreach (var areaToken in areaTokens)
                {
                    var allowedToken = areaToken.SelectToken("allowed");
                    var allowedEditorAliasToken = allowedToken.SelectToken($"[?(@ == '{allowedEditorAlias}')]");

                    if (deleteEditor)
                    {
                        if (allowedEditorAliasToken == null) continue;
                        allowedEditorAliasToken.Remove();
                    }
                    else
                    {
                        if (allowedEditorAliasToken != null) continue;
                        allowedToken.Last.AddAfterSelf(allowedEditorAlias);
                    }

                    preValueItem.Value.Value = parsedPreValue.ToString();
                }
            }
        }
    }
}