namespace Uintra20.Features.Search.Configuration
{
    public interface IElasticSettings
    {
        string SearchUrl { get; }
        int LimitBulkOperation { get; }
        int NumberOfShards { get; }
        int NumberOfReplicas { get; }
        string IndexName { get; }
        string SearchUserName { get; }
        string SearchPassword { get; }

    }
}