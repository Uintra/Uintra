using AutoMapper;

namespace Uintra20.Infrastructure.Extensions
{
    public static class AutoMapperExtensions
    {
        public static TDestination Map<TDestination>(this object obj) => 
            Mapper.Map<TDestination>(obj);

        public static TDestination Map<TSource, TDestination>(this TSource src, TDestination destination) =>
            Mapper.Map<TSource, TDestination>(src, destination);
    }
}