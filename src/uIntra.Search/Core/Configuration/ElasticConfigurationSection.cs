
namespace Uintra.Search.Configuration
{
    public class ElasticConfigurationSection : IElasticConfigurationSection
    {
        public static string SettingName => "Shared.Elastic5";

        public string Host { get; set; }

        public string Port { get; set; }

        public int? LimitBulkOperation { get; set; }

        public int? NumberOfShards { get; set; }

        public int? NumberOfReplicas { get; set; }

        public string IndexPrefix { get; set; }
    }
}