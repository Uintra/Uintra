using System.Collections.Generic;
using uIntra.Core.Controls;
using static Compent.uIntra.Core.Constants.DocumentTypeAliasConstants;

namespace Compent.uIntra.Core.Controls.EditorConfiguration
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