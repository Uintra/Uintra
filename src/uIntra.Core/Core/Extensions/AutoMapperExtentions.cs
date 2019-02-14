using AutoMapper;

namespace Uintra.Core.Extensions
{
    public static class AutoMapperExtensions
    {
        public static T Map<T>(this object obj) where T: class => Mapper.Map<T>(obj);
    }
}