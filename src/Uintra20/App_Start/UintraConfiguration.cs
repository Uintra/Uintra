using Compent.Shared.ConfigurationProvider.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UBaseline.Core.Startup;

namespace Uintra20
{
    public static class UintraConfiguration
    {
        public static IConfigurationBuilder AddUintraConfiguration(this IConfigurationBuilder configurationBuilder)
        {
            const string parentFolder = @"config\Uintra\";
            UBaselineConfiguration.AddConfiguration(configurationBuilder, $"{parentFolder}panelContainerSettings.json");
            return configurationBuilder;
        }
    }
}