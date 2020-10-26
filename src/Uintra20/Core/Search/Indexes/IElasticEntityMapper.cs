namespace Uintra20.Core.Search.Indexes
{
    public interface IElasticEntityMapper
    {
        bool CreateMap(out string error);
    }
}
