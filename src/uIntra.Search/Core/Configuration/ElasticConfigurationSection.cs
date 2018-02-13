using System.Configuration;

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
        public string IndexPrefix => (string)base[IndexPrefixKey];
    }
}