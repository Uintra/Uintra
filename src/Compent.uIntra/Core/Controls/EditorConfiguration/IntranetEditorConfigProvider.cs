using System.Collections.Generic;
using Uintra.Core.Controls;
using static Compent.Uintra.Core.Constants.DocumentTypeAliasConstants;

namespace Compent.Uintra.Core.Controls.EditorConfiguration
{
    public class IntranetEditorConfigProvider : EditorConfigProvider
    {
        protected override IEnumerable<string> GetAllowedAliasesForInternalLinkPicker()
        {
            return new[]
            {
                HomePage,
                ContentPage,
                BulletinsOverviewPage,
                NewsOverviewPage,
                EventsOverviewPage
            };
        }
    }
}