using AutoMapper;

namespace Uintra.Subscribe
{
    public class SubscribeAutoMapperProfiles : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Subscribe, SubscribeItemModel>();
        }
    }
}