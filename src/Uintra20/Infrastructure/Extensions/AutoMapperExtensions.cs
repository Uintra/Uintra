using AutoMapper;

namespace Uintra20.Infrastructure.Extensions
{
    public static class AutoMapperExtensions
    {
        public static TDestination Map<TDestination>(this object obj)
        {
            return Mapper.Map<TDestination>(obj);
        }
    }
}