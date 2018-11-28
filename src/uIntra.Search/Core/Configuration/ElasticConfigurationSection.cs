using System.Configuration;
using System.Management;
using static LanguageExt.Prelude;

namespace Uintra.Search.Configuration
{
    public class ElasticConfigurationSection : ConfigurationSection, IElasticConfigurationSection
    {
        private const string UrlKey = "url";
        private const string LimitBulkOperationKey = "limitBulkOperation";
        private const string NumberOfShardsKey = "numberOfShards";
        private const string NumberOfReplicasKey = "numberOfReplicas";
        private const string IndexPrefixKey = "indexPrefix";

        public static ElasticConfigurationSection Configuration => ConfigurationManager.GetSection("elasticConfiguration") as ElasticConfigurationSection;

        [ConfigurationProperty(UrlKey, IsRequired = true)]
        public string Url => (string)base[UrlKey];

        [ConfigurationProperty(LimitBulkOperationKey, DefaultValue = "1500")]
        public int LimitBulkOperation => (int)base[LimitBulkOperationKey];

        [ConfigurationProperty(NumberOfShardsKey, DefaultValue = "1")]
        public int NumberOfShards => (int)base[NumberOfShardsKey];

        [ConfigurationProperty(NumberOfReplicasKey, DefaultValue = "0")]
        public int NumberOfReplicas => (int)base[NumberOfReplicasKey];

        [ConfigurationProperty(IndexPrefixKey, IsRequired = false, DefaultValue = "")]
        public string IndexPrefix => 
            Optional((string)base[IndexPrefixKey])
                .IfNone(GetDriveId);

        private static string GetDriveId()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            var id = string.Empty;
            foreach (var managementObject in searcher.Get())
            {
                id = managementObject["SerialNumber"].ToString();
            }

            return id;
        }
    }
}