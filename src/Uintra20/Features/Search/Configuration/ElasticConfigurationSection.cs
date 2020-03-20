using System;
using System.Configuration;

namespace Uintra20.Features.Search.Configuration
{
    public class ElasticConfigurationSection : ConfigurationSection, IElasticConfigurationSection
    {
        private const string UrlKey = "url";
        private const string LimitBulkOperationKey = "limitBulkOperation";
        private const string NumberOfShardsKey = "numberOfShards";
        private const string NumberOfReplicasKey = "numberOfReplicas";
        private const string IndexPrefixKey = "indexPrefix";
        private const string UserNameKey = "username";
        private const string PasswordKey = "password";

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
        public string IndexPrefix => string.IsNullOrEmpty((string)base[IndexPrefixKey]) ? DateTime.Now.Ticks.ToString() : (string)base[IndexPrefixKey];

        [ConfigurationProperty(UserNameKey, IsRequired = false, DefaultValue = "")]
        public string UserName { get; set; }
        [ConfigurationProperty(PasswordKey, IsRequired = false, DefaultValue = "")]
        public string Password { get; set; }

    }
}