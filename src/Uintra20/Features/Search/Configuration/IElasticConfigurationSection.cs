namespace Uintra20.Features.Search.Configuration
{
    public interface IElasticConfigurationSection
    {
        string Url { get; }
        int LimitBulkOperation { get; }
        int NumberOfShards { get; }
        int NumberOfReplicas { get; }
        string IndexPrefix { get; }
        string UserName { get; set; }
        string Password { get; set; }

    }
}