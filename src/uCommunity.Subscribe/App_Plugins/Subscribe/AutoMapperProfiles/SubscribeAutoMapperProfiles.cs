using AutoMapper;
using uCommunity.Subscribe.App_Plugins.Subscribe.Model;

namespace uCommunity.Subscribe.App_Plugins.Subscribe.AutoMapperProfiles
{
    public class SubscribeAutoMapperProfiles : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Sql.Subscribe, SubscribeItemModel>();
        }
    }
}