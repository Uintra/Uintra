namespace Uintra.Search
{
    public interface IElasticEntityMapper
    {
        bool CreateMap(out string error);
    }
}
