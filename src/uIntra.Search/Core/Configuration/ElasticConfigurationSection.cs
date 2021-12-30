using Compent.Shared.Configuration.Builders.Azure.Attributes;

namespace Uintra.Search.Configuration
{
    [AzureAppConfiguration(SettingName = "Shared.Elastic5")]
    public class ElasticConfigurationSection : IElasticConfigurationSection
    {
        [AzureKeyVaultReference]
        public string Host { get; set; }

        [AzureKeyVaultReference]
        public string Port { get; set; }

        public int? LimitBulkOperation { get; set; }

        public int? NumberOfShards { get; set; }

        public int? NumberOfReplicas { get; set; }

        public string IndexPrefix { get; set; }
    }
}