namespace Uintra.Search.Configuration
{
    public interface IElasticConfigurationSection
    {
        string Host { get; }
        string Port { get; set; }
        int? LimitBulkOperation { get; }
        int? NumberOfShards { get; }
        int? NumberOfReplicas { get; }
        string IndexPrefix { get; }
    }
}