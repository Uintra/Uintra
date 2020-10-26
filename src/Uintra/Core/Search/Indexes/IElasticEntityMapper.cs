namespace Uintra.Core.Search.Indexes
{
    public interface IElasticEntityMapper
    {
        bool CreateMap(out string error);
    }
}
