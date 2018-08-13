using Compent.Uintra.Core.Constants;
using System.Collections.Generic;
using Uintra.Core.Controls;

namespace Compent.Uintra.Core.Controls.EditorConfiguration
{
    public class IntranetEditorConfigProvider : EditorConfigProvider
    {
        protected override IEnumerable<string> GetAllowedAliasesForInternalLinkPicker()
        {
            return new[]
            {
                DocumentTypeAliasConstants.HomePage,
                DocumentTypeAliasConstants.ContentPage,
                DocumentTypeAliasConstants.BulletinsOverviewPage,
                DocumentTypeAliasConstants.NewsOverviewPage,
                DocumentTypeAliasConstants.EventsOverviewPage
            };
        }
    }
}