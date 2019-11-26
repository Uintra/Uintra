﻿using Compent.Uintra.Core.Constants;
using System.Collections.Generic;
using Uintra.Core.ApplicationSettings;
using Uintra.Core.Controls;

namespace Compent.Uintra.Core.Controls.EditorConfiguration
{
    public class IntranetEditorConfigProvider : EditorConfigProvider
    {
	    private readonly IApplicationSettings _applicationSettings;

	    public IntranetEditorConfigProvider(IApplicationSettings applicationSettings)
	    {
		    _applicationSettings = applicationSettings;
	    }
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
        protected override string GetDefaultToolbar()
        {
	        return _applicationSettings.DefaultToolbarConfig;
        }
    }
}