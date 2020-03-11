namespace Uintra20.Features.Search
{
    public interface IElasticEntityMapper
    {
        bool CreateMap(out string error);
    }
}
