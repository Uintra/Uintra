using Compent.Shared.ConfigurationProvider.Contract;
using UBaseline.Core.Startup;

namespace Uintra20.Infrastructure.Configuration
{
    public static class UIntraConfiguration
    {
        private const string ConfigFolderPath = @"config\UIntra\";
        public static IConfigurationBuilder AddConfiguration(this IConfigurationBuilder configurationBuilder)
        {
            UBaselineConfiguration.AddConfiguration(configurationBuilder,
                $"{ConfigFolderPath}latestActivitySettings.json");

            return configurationBuilder;
        }
    }
}